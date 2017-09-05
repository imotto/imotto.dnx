using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace iMotto.Cache.RedisImpl
{
    public class RedisHelper
    {
        static ConnectionMultiplexer redis;

        static RedisHelper()
        {
            var //connstr = System.Configuration.ConfigurationManager.AppSettings.Get("REDIS_CONNSTR");
            //if (string.IsNullOrWhiteSpace(connstr))
            //{
                connstr = "127.0.0.1,password=fooredis";
            //}

            redis = ConnectionMultiplexer.Connect(connstr);
        }

        public static IDatabase GetDatabase()
        {
            return redis.GetDatabase();
        }

        //public static void Subscribe(string channel, Action<RedisChannel, RedisValue> handler)
        //{
        //    var sub = redis.GetSubscriber();
        //    sub.Subscribe(channel, handler);
        //}

        public static bool StringSet(string key, string value, TimeSpan? expires = null)
        {
            var db = redis.GetDatabase();
            return db.StringSet(key, value, expires);
        }

        public static string StringGet(string key)
        {
            var db = redis.GetDatabase();
            return db.StringGet(key);
        }

        public static string GetHashValue(string key, string field)
        {
            var db = redis.GetDatabase();
            return db.HashGet(key, field);
        }

        public static bool SetContains(string key, string value)
        {
            var db = redis.GetDatabase();

            return db.SetContains(key, value);

        }

        public static long SetAdd(string key, RedisValue[] values, TimeSpan? expire = null)
        {
            var db = redis.GetDatabase();
            
            var result= db.SetAdd(key, values);
            if (expire.HasValue)
            {
                db.KeyExpire(key, expire);
            }

            return result;

        }

        public static bool SetAdd(string key, string value, TimeSpan? expire = null)
        {
            var db = redis.GetDatabase();
            var result =  db.SetAdd(key, value);
            if (expire.HasValue)
            {
                db.KeyExpire(key, expire);
            }
            return result;
        }

        internal static bool SetRemove(string key, string value)
        {
            var db = redis.GetDatabase();
            return db.SetRemove(key, value);
        }

        public static RedisValue[] SetMembers(string key)
        {
            var db = redis.GetDatabase();
            var result = db.SetMembers(key);

            return result;
        }

        public static void HashSet(string key, HashEntry[] values, TimeSpan? expires=null)
        {
            var db = redis.GetDatabase();
            db.HashSet(key, values);
            if (expires.HasValue)
            {
                db.KeyExpire(key, expires);
            }
        }

        public static HashEntry[] HashGetAll(string key)
        {
            var db = redis.GetDatabase();
            return db.HashGetAll(key);
        }

        public static long HashIncrement(string key, string field, long value=1)
        {
            var db = redis.GetDatabase();
            return db.HashIncrement(key, field, value);
        }

        public static long HashDecrement(string key, string field, long value = 1)
        {
            var db = redis.GetDatabase();
            
            return db.HashDecrement(key, field, value);
        }

        /// <summary>
        /// 在SortedSet中加入新项，或更新已有项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd(string key, string member, double score)
        {
            var db = redis.GetDatabase();
            return db.SortedSetAdd(key, member, score);
        }

        public static bool KeyDelete(string key)
        {
            var db = redis.GetDatabase();

            return db.KeyDelete(key);
        }
    }
}
