using iMotto.Common.Settings;
using StackExchange.Redis;
using System;


namespace iMotto.Cache.RedisImpl
{

    public class RedisHelper
    {
        private readonly ConnectionMultiplexer _multiplexer;

        public RedisHelper(ISettingProvider settingProvider)
        {
            var cacheSetting = settingProvider.GetCacheSetting();
            _multiplexer = ConnectionMultiplexer.Connect(cacheSetting.RedisConnStr);
        }

        public IDatabase GetDatabase()
        {
            return _multiplexer.GetDatabase();
        }
        

        //public static void Subscribe(string channel, Action<RedisChannel, RedisValue> handler)
        //{
        //    var sub = redis.GetSubscriber();
        //    sub.Subscribe(channel, handler);
        //}

        public bool StringSet(string key, string value, TimeSpan? expires = null)
        {
            var db = _multiplexer.GetDatabase();
            return db.StringSet(key, value, expires);
        }

        public string StringGet(string key)
        {
            var db = _multiplexer.GetDatabase();
            return db.StringGet(key);
        }

        public string GetHashValue(string key, string field)
        {
            var db = _multiplexer.GetDatabase();
            return db.HashGet(key, field);
        }

        public bool SetContains(string key, string value)
        {
            var db = _multiplexer.GetDatabase();

            return db.SetContains(key, value);

        }

        public long SetAdd(string key, RedisValue[] values, TimeSpan? expire = null)
        {
            var db = _multiplexer.GetDatabase();
            
            var result= db.SetAdd(key, values);
            if (expire.HasValue)
            {
                db.KeyExpire(key, expire);
            }

            return result;

        }

        public bool SetAdd(string key, string value, TimeSpan? expire = null)
        {
            var db = _multiplexer.GetDatabase();
            var result =  db.SetAdd(key, value);
            if (expire.HasValue)
            {
                db.KeyExpire(key, expire);
            }
            return result;
        }

        internal bool SetRemove(string key, string value)
        {
            var db = _multiplexer.GetDatabase();
            return db.SetRemove(key, value);
        }

        public RedisValue[] SetMembers(string key)
        {
            var db = _multiplexer.GetDatabase();
            var result = db.SetMembers(key);

            return result;
        }

        public void HashSet(string key, HashEntry[] values, TimeSpan? expires=null)
        {
            var db = _multiplexer.GetDatabase();
            db.HashSet(key, values);
            if (expires.HasValue)
            {
                db.KeyExpire(key, expires);
            }
        }

        public HashEntry[] HashGetAll(string key)
        {
            var db = _multiplexer.GetDatabase();
            return db.HashGetAll(key);
        }

        public long HashIncrement(string key, string field, long value=1)
        {
            var db = _multiplexer.GetDatabase();
            return db.HashIncrement(key, field, value);
        }

        public long HashDecrement(string key, string field, long value = 1)
        {
            var db = _multiplexer.GetDatabase();
            
            return db.HashDecrement(key, field, value);
        }

        /// <summary>
        /// 在SortedSet中加入新项，或更新已有项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd(string key, string member, double score)
        {
            var db = _multiplexer.GetDatabase();
            return db.SortedSetAdd(key, member, score);
        }

        public bool KeyDelete(string key)
        {
            var db = _multiplexer.GetDatabase();

            return db.KeyDelete(key);
        }
    }
}
