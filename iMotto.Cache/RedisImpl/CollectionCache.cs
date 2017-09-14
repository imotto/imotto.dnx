using System;
using iMotto.Events;
using iMotto.Data.Entities;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Cache.RedisImpl
{
    class CollectionCache : ICollectionCache
    {
        private const string KEY_COLLECTION_FMT = "COL{0}";
        private const string KEY_COLLECTION_RANK = "COLRANK";
        private const string KEY_COLLECTION_MOTTO_FMT = "CMS{0}";
        private readonly RedisHelper _redisHelper;

        public CollectionCache(RedisHelper redisHelper)
        {
            _redisHelper = redisHelper;
        }

        public async Task<List<long>> WhichHasContainsMID(long mid, IEnumerable<long> cids)
        {
            var redis = _redisHelper.GetDatabase();

            var batch = redis.CreateBatch();

            var result = new List<long>();
            var tasks = new List<Task>();

            foreach (var item in cids)
            {
                var task= batch.SetContainsAsync(string.Format(KEY_COLLECTION_MOTTO_FMT, item.ToString()), mid).ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        result.Add(item);
                    }
                });

                tasks.Add(task);
            }

            batch.Execute();
            await Task.WhenAll(tasks);

            return result;
        }

        public void HandleEvent(UnLoveCollectionEvent @event)
        {
            _redisHelper.HashDecrement(string.Format(KEY_COLLECTION_FMT,
                @event.LoveCollection.CID), F_LOVES);
        }

        public void HandleEvent(UnCollectMottoEvent @event)
        {
            var redis = _redisHelper.GetDatabase();

            redis.HashDecrement(string.Format(KEY_COLLECTION_FMT,
                @event.CollectionMotto.CID.ToString()), F_MOTTOS);
            
            redis.SetRemove(string.Format(KEY_COLLECTION_MOTTO_FMT, @event.CollectionMotto.CID),
                @event.CollectionMotto.MID.ToString());
        }

        public void HandleEvent(CollectMottoEvent @event)
        {
            var redis = _redisHelper.GetDatabase();

            redis.HashIncrement(string.Format(KEY_COLLECTION_FMT,
                @event.CollectionMotto.CID.ToString()), F_MOTTOS);

            redis.SetAdd(string.Format(KEY_COLLECTION_MOTTO_FMT, @event.CollectionMotto.CID),
                @event.CollectionMotto.MID.ToString());
        }

        public void HandleEvent(LoveCollectionEvent @event)
        {
            _redisHelper.HashIncrement(string.Format(KEY_COLLECTION_FMT,
                @event.LoveCollection.CID.ToString()), F_LOVES);
        }

        public void HandleEvent(CreateCollectionEvent @event)
        {
            var redis = _redisHelper.GetDatabase();
            var collection = @event.Collection;
            var key = string.Format(KEY_COLLECTION_FMT, collection.ID);
            var entries = ConvertToEntries(collection);

            redis.HashSet(key, entries);
            redis.KeyExpire(key, TimeSpan.FromDays(14));
            redis.SortedSetAdd(KEY_COLLECTION_RANK, collection.ID.ToString(), 0);
        }

        private const string F_CREATETIME = "CTIME";
        private const string F_UID = "UID";
        private const string F_TITLE = "TITLE";
        private const string F_DESCRIPTION = "DESC";
        private const string F_MOTTOS = "MOTTO";
        private const string F_LOVES = "LOVE";
        private const string F_TAGS = "TAG";

        private static HashEntry[] ConvertToEntries(Collection collection)
        {
            HashEntry[] entries = new HashEntry[7];
            entries[0] = new HashEntry(F_CREATETIME, collection.CreateTime.ToString());
            entries[1] = new HashEntry(F_UID, collection.UID);
            entries[2] = new HashEntry(F_TITLE, collection.Title);
            entries[3] = new HashEntry(F_DESCRIPTION, collection.Description);
            entries[4] = new HashEntry(F_MOTTOS, collection.Mottos.ToString());
            entries[5] = new HashEntry(F_LOVES, collection.Loves.ToString());
            entries[6] = new HashEntry(F_TAGS, collection.Tags);

            return entries;
        }

        private static Collection ConvertFromEntries(HashEntry[] entries)
        {
            var col = new Collection();
            foreach (var item in entries)
            {
                switch (item.Name)
                {
                    case F_CREATETIME:col.CreateTime = Convert.ToDateTime((string)item.Value);break;
                    case F_DESCRIPTION:col.Description = item.Value;break;
                    case F_LOVES:col.Loves = (int)item.Value;break;
                    case F_MOTTOS:col.Mottos = (int)item.Value;break;
                    case F_TAGS:col.Tags = item.Value;break;
                    case F_TITLE:col.Title = item.Value;break;
                    case F_UID:col.UID = item.Value;break;
                    default:break;
                }
            }

            return col;
        }

    }
}
