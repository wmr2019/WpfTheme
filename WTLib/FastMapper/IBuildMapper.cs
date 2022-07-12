namespace WTLib.FastMapper
{
    public interface IBuildMapper
    {
    }

    public interface IEmitBuildMapper<in TSource, out TTarget> : IBuildMapper
    {
        TTarget Map(TSource source);
    }

    public interface IUnsafeBuildMapper<in TSource, out TTarget> : IBuildMapper
    {
        TTarget Map(TSource source);
    }
}
