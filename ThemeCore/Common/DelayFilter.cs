using System;

namespace ThemeCore.Common
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
