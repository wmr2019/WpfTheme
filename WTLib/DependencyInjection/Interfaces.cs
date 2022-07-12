using System;

namespace WTLib.DependencyInjection
{
    public class IocContainerResolverException : Exception
    {
        public IocContainerResolverException(string message)
            : base(message)
        {

        }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
    }

    public enum Lifestyle : byte
    {
        Transient = 0,
        Singleton = 1,
        Scoped = 2,
    }

    public interface IObjectResolver
    {
        IScopedObjectResolver BeginScope(ScopeProvider provider);
        Lifestyle Lifestyle(Type type);
        T Resolve<T>();
        object Resolve(Type type);
    }

    public interface IScopedObjectResolver : IObjectResolver, IDisposable
    {
    }
}