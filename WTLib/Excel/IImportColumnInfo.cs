﻿using System;

namespace WTLib.Excel
{
    public interface IImportColumnInfo<T> where T : class
    {
        Action<T, object> SetDataValue { get; set; }
        Func<object, bool> MatchDataValue { get; set; }
        string Name { get; set; }
    }
}
