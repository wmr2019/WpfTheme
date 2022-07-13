/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Common
*   文件名称 ：DelayFilter.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 3:48:22 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;

namespace ThemeMetro.Common
{
    public class DelayFilter
    {
        static DelayFilter @default = new DelayFilter();

        public static DelayFilter Default
        {
            get { return @default; }
            set
            {
                @default = value ?? throw new ArgumentNullException("value");
            }
        }

        public virtual Func<object, bool> GetFilter(string query, Func<object, string> stringFromItem)
        {
            return item =>
            {
                if (string.IsNullOrEmpty(query?.Trim()))
                {
                    // 当查询条件为空时显示所有
                    return true;
                }

                var value = stringFromItem(item);
                var filter = query.Trim();
                if (value.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    return true;

                var spell = ChineseParser.GetFirstLetter(value);
                if (spell == null)
                    return false;

                return spell.Contains(filter.ToUpper());
            };
        }

        public virtual Func<object, object> Find(string query, Func<object, string> stringFromItem)
        {
            return item =>
            {
                if (string.IsNullOrEmpty(query))
                    return null;
                var value = stringFromItem(item);
                if (value == query)
                {
                    return item;
                }
                return null;
            };
        }

        public virtual int MaxSuggestionCount
        {
            get { return 200; }
        }

        public virtual TimeSpan Delay
        {
            get { return TimeSpan.FromMilliseconds(30.0); }
        }
    }
}
