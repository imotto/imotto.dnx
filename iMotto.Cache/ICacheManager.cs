namespace iMotto.Cache
{
    public interface ICacheManager
    {
        T GetCache<T>() where T : class;
    }
}