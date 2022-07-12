using System;

namespace WTLib.FastMapper
{
    public struct TypePair : IEquatable<TypePair>
    {
        public Type SourceType { get; }

        public Type DestinationType { get; }

        public TypePair(Type sourceType, Type destinationType)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
        }

        public static TypePair Create<TSource, TDestination>()
        {
            return new TypePair(typeof(TSource), typeof(TDestination));
        }

        public bool Equals(TypePair other)
        {
            if (this.SourceType == other.SourceType)
                return this.DestinationType == other.DestinationType;
            return false;
        }

        public override bool Equals(object other)
        {
            if (other is TypePair pair)
                return this.Equals(pair);
            return false;
        }

        public override int GetHashCode()
        {
            int hash1 = SourceType.GetHashCode();
            int hash2 = DestinationType.GetHashCode();
            return hash1 << 5 + hash1 ^ hash2;
        }
    }
}
