using System;
using System.Collections.Generic;

namespace WTLib.FastMapper
{
    public sealed class EmitMapper : IMapper
    {
        private EmitMapper() { }

        private readonly Dictionary<TypePair, IBuildMapper> _emitBuildMapperCache
            = new Dictionary<TypePair, IBuildMapper>();

        public EmitMapper(IEnumerable<(Type, Type)> typePairs)
        {
            foreach (var (sourceType, targetType) in typePairs)
            {
                _emitBuildMapperCache.Add(new TypePair(sourceType, targetType),
                    MapperBuilder.CreateEmitBuildMapper(sourceType, targetType));
            }
        }

        public TTarget Map<TSource, TTarget>(TSource source)
        {
            var pair = TypePair.Create<TSource, TTarget>();
            if (_emitBuildMapperCache.TryGetValue(pair, out IBuildMapper mapper))
                return ((IEmitBuildMapper<TSource, TTarget>)mapper).Map(source);
            throw new InvalidOperationException("unregistered map.");
        }
    }
}
