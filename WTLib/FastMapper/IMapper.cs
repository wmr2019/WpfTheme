namespace WTLib.FastMapper
{
    public interface IMapper
    {
        TTarget Map<TSource, TTarget>(TSource source);
    }
}
