using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iMotto.Data.Entities;
using iMotto.Events;
using StackExchange.Redis;
using iMotto.Common;

namespace iMotto.Cache.RedisImpl
{
    class EvaluatingMottoCache : IEvaluatingMottoCache
    {
        private const string KEY_EVAL_MOTTO_FMT = "EVM{0}{1}";
        private const string KEY_RANK_MOTTO_FMT = "RNM{0}";
        private const string F_ADDTIME = "ADDTIME";
        private const string F_CONTENT = "CONTENT";
        private const string F_DOWN = "DOWN";
        private const string F_LOVES = "LOVE";
        private const string F_RECRUIT_ID = "RID";
        private const string F_RECRUIT_TITLE = "RTITLE";
        private const string F_REVIEWS = "REVIEW";
        private const string F_UID = "UID";
        private const string F_SCORE = "SCORE";
        private const string F_UP = "UP";

        public Motto FindMotto(int theDay, long mid)
        {
            var entries = RedisHelper.HashGetAll(string.Format(KEY_EVAL_MOTTO_FMT, theDay, mid));
            if (entries != null && entries.Length > 0)
            {
                Motto motto = ConvertFromHashEntries(entries);
                motto.ID = mid;
                return motto;
            }

            return null;
        }

        public List<Motto> GetMottos(int theDay, Func<DateTime, int, int, List<Motto>> mottoLoader = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Motto[]> GetMottos(int theDay, int page, int pSize)
        {
            var db = RedisHelper.GetDatabase();
            var ids = db.SortedSetRangeByScore(string.Format(KEY_RANK_MOTTO_FMT, theDay.ToString()),
                order: Order.Descending,
                skip: (page - 1) * pSize,
                take: pSize);

            if (ids != null && ids.Length > 0)
            {
                var batch = db.CreateBatch();
                var tasks = new List<Task<Motto>>();
                foreach (var item in ids)
                {
                    var t = batch.HashGetAllAsync(string.Format(KEY_EVAL_MOTTO_FMT, theDay.ToString(), (string)item)).ContinueWith(tresult =>
                    {
                        if (tresult.IsCompleted)
                        {
                            if (tresult.Result != null && tresult.Result.Length > 0)
                            {
                                var motto = ConvertFromHashEntries(tresult.Result);
                                motto.ID = Convert.ToInt64(item);
                                return motto;
                            }
                        }

                        return null;
                    });

                    tasks.Add(t);
                }

                batch.Execute();

                return await Task.WhenAll(tasks);
            }

            return null;
        }

        public void HandleEvent(CreateReviewEvent @event)
        {
            if (Utils.GetTheDay(DateTime.Now.AddDays(-10)) < @event.TheDay)
            {
                RedisHelper.HashIncrement(
                string.Format(KEY_EVAL_MOTTO_FMT, @event.TheDay, @event.Review.MID),
                F_REVIEWS);
            }
        }

        public void HandleEvent(RemoveReviewEvent @event)
        {
            if (Utils.GetTheDay(DateTime.Now.AddDays(-10)) < @event.TheDay)
            {
                RedisHelper.HashDecrement(
                string.Format(KEY_EVAL_MOTTO_FMT, @event.TheDay, @event.Review),
                F_REVIEWS);
            }
        }

        public void HandleEvent(UnloveMottoEvent @event)
        {
            if (Utils.GetTheDay(DateTime.Now.AddDays(-10)) < @event.LoveMotto.TheDay)
            {
                RedisHelper.HashDecrement(string.Format(KEY_EVAL_MOTTO_FMT, @event.LoveMotto.TheDay, @event.LoveMotto.MID),
                    F_LOVES);
            }
        }

        public void HandleEvent(LoveMottoEvent @event)
        {
            if (Utils.GetTheDay(DateTime.Now.AddDays(-10)) < @event.LoveMotto.TheDay)
            {
                var redis = RedisHelper.GetDatabase();
                redis.HashIncrement(string.Format(KEY_EVAL_MOTTO_FMT, @event.LoveMotto.TheDay, @event.LoveMotto.MID),
                    F_LOVES);
            }
        }

        public void HandleEvent(CreateVoteEvent @event)
        {
            var mkey = string.Format(KEY_EVAL_MOTTO_FMT, @event.TheDay, @event.MID);
            if (@event.Vote == 1)
            {  
                var entries = RedisHelper.HashGetAll(mkey);
                if (entries != null && entries.Length > 0)
                {
                    var motto = ConvertFromHashEntries(entries);
                    motto.Up += 1;
                    motto.Score = Utils.Hot(motto.Up, motto.Down, motto.AddTime);

                    RedisHelper.HashSet(mkey, new HashEntry[] { new HashEntry(F_UP, motto.Up), new HashEntry(F_SCORE, motto.Score)});

                    RedisHelper.SortedSetAdd(string.Format(KEY_RANK_MOTTO_FMT, @event.TheDay),
                        @event.MID.ToString(), motto.Score);
                }
            }
            else if (@event.Vote == -1)
            {
                var entries = RedisHelper.HashGetAll(mkey);
                if (entries != null && entries.Length > 0)
                {
                    var motto = ConvertFromHashEntries(entries);
                    motto.Down += 1;
                    motto.Score = Utils.Hot(motto.Up, motto.Down, motto.AddTime);

                    RedisHelper.HashSet(mkey, new HashEntry[] { new HashEntry(F_DOWN, motto.Down), new HashEntry(F_SCORE, motto.Score) });

                    RedisHelper.SortedSetAdd(string.Format(KEY_RANK_MOTTO_FMT, @event.TheDay),
                        @event.MID.ToString(), motto.Score);
                }
            }
        }

        public void HandleEvent(CreateMottoEvent @event)
        {
            var theday = @event.Motto.AddTime.ToString("yyyyMMdd");

            @event.Motto.Score = Utils.Hot(@event.Motto.Up, @event.Motto.Down, @event.Motto.AddTime);

            var entries = ConvertToHashEntries(@event.Motto);
            RedisHelper.HashSet(string.Format(KEY_EVAL_MOTTO_FMT, theday, @event.Motto.ID),
                entries,
                TimeSpan.FromDays(14));

            RedisHelper.SortedSetAdd(string.Format(KEY_RANK_MOTTO_FMT, theday), @event.Motto.ID.ToString(), @event.Motto.Score);

        }

        private static HashEntry[] ConvertToHashEntries(Motto motto)
        {
            HashEntry[] entries = new HashEntry[10];

            entries[0] = new HashEntry(F_ADDTIME, motto.AddTime.ToString());
            entries[1] = new HashEntry(F_CONTENT, motto.Content.ToString());
            entries[2] = new HashEntry(F_DOWN, motto.Down.ToString());
            entries[3] = new HashEntry(F_LOVES, motto.Loves.ToString());
            entries[4] = new HashEntry(F_RECRUIT_ID, motto.RecruitID.ToString());
            entries[5] = new HashEntry(F_RECRUIT_TITLE, motto.RecruitTitle.ToString());
            entries[6] = new HashEntry(F_REVIEWS, motto.Reviews.ToString());
            entries[7] = new HashEntry(F_SCORE, motto.Score.ToString());
            entries[8] = new HashEntry(F_UID, motto.UID.ToString());
            entries[9] = new HashEntry(F_UP, motto.Up.ToString());

            return entries;
        }

        private Motto ConvertFromHashEntries(HashEntry[] entries)
        {
            var motto = new Motto();
            foreach (var item in entries)
            {
                switch (item.Name)
                {
                    case F_ADDTIME: motto.AddTime = Convert.ToDateTime((string)item.Value); break;
                    case F_CONTENT: motto.Content = item.Value; break;
                    case F_DOWN: motto.Down = (int)item.Value; break;
                    case F_LOVES: motto.Loves = (int)item.Value; break;
                    case F_RECRUIT_ID: motto.RecruitID = (int)item.Value; break;
                    case F_RECRUIT_TITLE: motto.RecruitTitle = (string)item.Value; break;
                    case F_REVIEWS: motto.Reviews = (int)item.Value; break;
                    case F_SCORE: motto.Score = (double)item.Value; break;
                    case F_UID: motto.UID = (string)item.Value; break;
                    case F_UP: motto.Up = (int)item.Value; break;
                    default: break;
                }
            }

            return motto;
        }

 
    }
}
