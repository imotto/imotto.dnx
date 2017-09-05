namespace iMotto.Cache
{
    public interface ISyncRootCache
    {
        bool AcquireSyncLock(string type, int giftId);
        void ReleaseSyncLock(string type, int giftId);
    }
}
