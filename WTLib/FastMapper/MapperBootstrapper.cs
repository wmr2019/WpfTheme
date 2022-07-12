using System;

namespace WTLib.FastMapper
{
    public sealed class MapperBootstrapper
    {
        private readonly MapperConfigration _mapperConfigration = new MapperConfigration();

        private MapperBootstrapper() { }

        public MapperBootstrapper(Action<MapperConfigration> config)
        {
            config(_mapperConfigration);
        }

        public IMapper CreateEmitMapper()
        {
            return new EmitMapper(_mapperConfigration.MapTypePairs);
        }

        public IMapper CreateUnsafeMapper()
        {
            return new UnsafeMapper(_mapperConfigration.MapTypePairs);
        }
    }
}
