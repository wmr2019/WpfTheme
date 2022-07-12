using System;

namespace WTLib.Excel
{
    public class ImportColumnInfo<T> : IImportColumnInfo<T> where T : class
    {
        public Action<T, object> SetDataValue { get; set; }
        public Func<object, bool> MatchDataValue { get; set; }
        public string Name { get; set; }
    }
}
