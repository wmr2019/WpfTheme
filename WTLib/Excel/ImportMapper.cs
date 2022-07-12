using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WTLib.Excel
{
    /// <summary>
    /// import excel-object mapper
    /// </summary>
    /// <typeparam name="T">model</typeparam>
    public sealed class ImportMapper<T> where T : class, new()
    {
        private readonly IDictionary<int, IImportColumnInfo<T>> _mapCache;

        public string FilePath { get; private set; }
        public string ErrorInfo { get; private set; }

        public ImportMapper()
        {
            _mapCache = new Dictionary<int, IImportColumnInfo<T>>();
        }

        public ImportMapper<T> Map(int columnIndex, Action<T, object> setter, Func<object, bool> match)
        {
            var column = CreateOrAddColumnInfo(columnIndex);
            column.MatchDataValue = match;
            column.SetDataValue = setter;
            return this;
        }

        public ImportMapper<T> Map(int columnIndex, string columnName, Action<T, object> setter, Func<object, bool> match)
        {
            var column = CreateOrAddColumnInfo(columnIndex, columnName);
            column.MatchDataValue = match;
            column.SetDataValue = setter;
            return this;
        }

        // base type match
        public ImportMapper<T> Map(int columnIndex, Action<T, object> setter, Type columnType, bool notNull = false)
        {
            return Map(columnIndex, setter, val =>
             {
                 if (notNull)
                 {
                     if (val == null)
                         return false;
                 }
                 if (columnType == typeof(string))
                     return true;
                 if (columnType.IsNumeric())
                 {
                     val = val.AsDouble();
                     return true;
                 }
                 if (columnType == typeof(DateTime))
                 {
                     val = val.AsDateTime();
                     return true;
                 }
                 if (columnType == typeof(bool))
                 {
                     val = val.AsBool();
                     return true;
                 }
                 return false;
             });
        }

        public IEnumerable<T> Match(string filePath, string sheetName = null)
        {
            FilePath = filePath;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                if (string.IsNullOrEmpty(sheetName))
                    return Match(workbook, 0);
                else
                    return Match(workbook, workbook.GetSheetIndex(sheetName));
            }
        }

        public IEnumerable<T> Match(string filePath, int sheetIndex)
        {
            FilePath = filePath;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                return Match(workbook, sheetIndex);
            }
        }

        public void Clear()
        {
            if (_mapCache != null)
                _mapCache.Clear();
        }

        private IEnumerable<T> Match(IWorkbook workbook, int sheetIndex)
        {
            ErrorInfo = null;
            int rowIndex = 0;
            int columnIndex = 0;
            var results = new List<T>();
            try
            {
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                if (CheckColumn(sheet))
                    return results;
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null && !CheckBlankRow(row))
                    {
                        T t = new T();
                        foreach (var key in _mapCache.Keys)
                        {
                            ICell cell = row.GetCell(key);
                            columnIndex = key;
                            rowIndex = i;
                            object val = null;
                            if (cell != null)
                                val = cell.CellType == CellType.Formula
                                    ? GetValueFromCellType(cell, cell.CachedFormulaResultType)
                                    : GetValueFromCellType(cell, cell.CellType);
                            var column = _mapCache[key];
                            if (column.MatchDataValue(val))
                                column.SetDataValue(t, val);
                            // else throw new ImportMatchException(rowIndex, columnIndex, FilePath, Properties.Resources.Cell_Type_Not_Match);
                        }
                        results.Add(t);
                    }
                }
                return results;
            }
            catch (Exception e)
            {
                throw new ImportMatchException(rowIndex, columnIndex, FilePath, e.Message, e);
            }
        }

        private bool CheckColumn(ISheet sheet)
        {
            if (!_mapCache.Values.Any(i => !string.IsNullOrEmpty(i.Name)))
            {
                return false;
            }

            IRow row = sheet.GetRow(sheet.FirstRowNum);
            if (row == null || CheckBlankRow(row))
            {
                ErrorInfo = Properties.Resources.Model_Not_Match;
                return true;
            }

            foreach (var key in _mapCache.Keys)
            {
                var column = _mapCache[key];
                if (string.IsNullOrEmpty(column.Name))
                    continue;

                ICell cell = row.GetCell(key);
                if (cell == null)
                {
                    ErrorInfo = Properties.Resources.Model_Not_Match;
                    return true;
                }
                var val = cell.CellType == CellType.Formula
                    ? GetValueFromCellType(cell, cell.CachedFormulaResultType)
                    : GetValueFromCellType(cell, cell.CellType);

                if (val == null)
                {
                    ErrorInfo = Properties.Resources.Model_Not_Match;
                    return true;
                }
                if (!StringComparer.OrdinalIgnoreCase.Equals(column.Name, val.ToString().Trim()))
                {
                    ErrorInfo = Properties.Resources.Model_Not_Match;
                    return true;
                }
            }

            return false;
        }

        private bool CheckBlankRow(IRow row)
        {
            bool blank = true;
            foreach (var key in _mapCache.Keys)
            {
                ICell cell = row.GetCell(key);
                if (!(cell == null || cell.CellType == CellType.Blank))
                {
                    blank = false;
                    break;
                }
            }
            return blank;
        }

        private object GetValueFromCellType(ICell cell, CellType cellType)
        {
            switch (cellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Error:
                    throw new ImportMatchException(cell.RowIndex, cell.ColumnIndex, FilePath, Properties.Resources.Error_Cell);
                case CellType.Unknown:
                    throw new ImportMatchException(cell.RowIndex, cell.ColumnIndex, FilePath, Properties.Resources.Error_Cell);
                default:
                    return null;
            }
        }

        internal IImportColumnInfo<T> CreateOrAddColumnInfo(int columnIndex, string columnName = null)
        {
            if (_mapCache.ContainsKey(columnIndex))
            {
                return _mapCache[columnIndex];
            }
            var column = new ImportColumnInfo<T>() { Name = columnName };
            _mapCache.Add(columnIndex, column);
            return column;
        }
    }
}
