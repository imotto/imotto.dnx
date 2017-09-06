namespace iMotto.Cache
{
    public interface ICacheManager
    {
        void Initialize();

        T GetCache<T>() where T : class;
    }
}