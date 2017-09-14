using System;

namespace iMotto.Cache.RedisImpl
{
    class SyncRootCache : ISyncRootCache
    {
        private const string SYNC_ROOT_KEY_FMT = "sync_{0}_{1}";
        private readonly RedisHelper _redisHelper;

        public SyncRootCache(RedisHelper redisHelper)
        {
            _redisHelper = redisHelper;
        }

        public bool AcquireSyncLock(string type, int giftId)
        {
            var key = string.Format(SYNC_ROOT_KEY_FMT, type, giftId);
            var redis = _redisHelper.GetDatabase();
            return redis.StringSet(key, 0, TimeSpan.FromSeconds(60), when: StackExchange.Redis.When.NotExists);
        }

        public void ReleaseSyncLock(string type, int giftId)
        {
            var key = string.Format(SYNC_ROOT_KEY_FMT, type, giftId);
            var redis = _redisHelper.GetDatabase();
            redis.KeyDelete(key);
        }
    }
}
