using System;
using System.Collections.Generic;

namespace WTLib.FastMapper
{
    public sealed class MapperConfigration
    {
        internal readonly HashSet<(Type, Type)> MapTypePairs = new HashSet<(Type, Type)>();

        public void RegisterMap<TSource, TTarget>()
        {
            RegisterMap(typeof(TSource), typeof(TTarget));
        }

        public void RegisterMap(Type sourceType, Type targeType)
        {
            if (!MapTypePairs.Contains((sourceType, targeType)))
                MapTypePairs.Add((sourceType, targeType));
        }
    }
}
