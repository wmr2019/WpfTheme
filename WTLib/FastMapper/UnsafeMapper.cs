using System;
using System.Collections.Generic;

namespace WTLib.FastMapper
{
    public sealed class UnsafeMapper : IMapper
    {
        private UnsafeMapper()
        {
        }

        private readonly Dictionary<TypePair, IBuildMapper> _unsafeBuildMapperCache
            = new Dictionary<TypePair, IBuildMapper>();

        public UnsafeMapper(IEnumerable<(Type, Type)> typePairs)
        {
            foreach (var (sourceType, targetType) in typePairs)
            {
                _unsafeBuildMapperCache.Add(new TypePair(sourceType, targetType),
                    MapperBuilder.CreateUnsafeBuildMapper(sourceType, targetType));
            }
        }

        public TTarget Map<TSource, TTarget>(TSource source)
        {
            var pair = TypePair.Create<TSource, TTarget>();
            if (_unsafeBuildMapperCache.TryGetValue(pair, out IBuildMapper mapper))
                return ((IUnsafeBuildMapper<TSource, TTarget>)mapper).Map(source);
            throw new InvalidOperationException("unregistered map.");
        }

    }
}
