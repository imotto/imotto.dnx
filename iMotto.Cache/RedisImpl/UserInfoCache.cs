using iMotto.Data.Entities;
using iMotto.Events;
using iMotto.Data.Entities.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Cache.RedisImpl
{
    class UserInfoCache : IUserInfoCache
    {
        private const String CHANNEL_GENERAL_MSG = "CHANNEL_GENERAL_MSG";

        private const string KEY_USER_ALL_LOVED_MOTTOS_FMT = "UALM{0}";
        private const string KEY_USER_ALL_LOVED_COLLECTIONS_FMT = "UALC{0}";
        private const string KEY_USER_ALL_MOTTOS_FMT = "UGM{0}";
        private const string KEY_USER_ALL_COLLECTIONS_FMT = "UGC{0}";
        private const string KEY_USER_ALL_COLLECTED_MOTTOS_FMT = "UACM{0}";
        //private const string KEY_USER_ALL_SUPPORT_MOTTOS_FMT = "UASM{0}";
        //private const string KEY_USER_ALL_OPPOSE_MOTTOS_FMT = "UAOM{0}";
        private const string KEY_USER_ALL_VOTE_FMT = "UAMV{0}";
        private const string KEY_USER_ALL_REVIEW_MOTTOS_FMT = "UARM{0}";
        private const string KEY_USER_ALL_LOVED_USERS_FMT = "UALU{0}";
        private const string KEY_USER_ALL_BAN_USERS_FMT = "UABU{0}";
        //private const string KEY_USER_RANK_FMT = "URNK{0}";

        private const string KEY_USER_INFO_FMT = "UIF{0}";
        private const string F_USER_NAME = "USERNAME";
        private const string F_DISPLAY_NAME = "DISPLAYNAME";
        private const string F_THUMB = "THUMB";
        private const string F_SEX = "SEX";
        private const string F_S_COLLECTIONS = "S_ALBUM";
        private const string F_S_MOTTOS = "S_MOTTO";
        private const string F_S_RECRUITS = "S_RECRUIT";
        private const string F_S_REVIEWS = "S_REVIEW";
        private const string F_S_VOTES = "S_VOTE";
        private const string F_S_LOVED_COLLECTIONS = "S_LOVEDALBUM";
        private const string F_S_LOVED_MOTTOS = "S_LOVEDMOTTO";
        private const string F_S_FOLLOWERS = "S_FOLLOWER";
        private const string F_S_FOLLOWS = "S_FOLLOW";
        private const string F_S_BANS = "S_BAN";
        private const string F_S_REVENUE = "S_REVENUE";
        private const string F_S_BALANCE = "S_BALANCE";

        public User GetUserById(string uid)
        {
            var redis = RedisHelper.GetDatabase();
            var entries = redis.HashGetAll(string.Format(KEY_USER_INFO_FMT, uid));

            if (entries != null && entries.Length >= 16) //如果用户的信息数缺失时，将会重新加载用户信息
            {
                var user = ConvertFromHashEntries(entries);
                user.Id = uid;
                return user;
            }

            return null;
        }

        public void HandleEvent(LoveMottoEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            redis.HashIncrement(string.Format(KEY_USER_INFO_FMT, @event.LoveMotto.UID),
                F_S_LOVED_MOTTOS);

            redis.SetAdd(string.Format(KEY_USER_ALL_LOVED_MOTTOS_FMT, @event.LoveMotto.UID),
                @event.LoveMotto.MID.ToString());
        }

        public void HandleEvent(CreateReviewEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            redis.HashIncrement(string.Format(KEY_USER_INFO_FMT, @event.Review.UID),
                F_S_REVIEWS);

            redis.SetAdd(string.Format(KEY_USER_ALL_REVIEW_MOTTOS_FMT, @event.Review.UID),
                @event.Review.MID.ToString());
        }

        public void HandleEvent(RemoveReviewEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            redis.HashDecrement(string.Format(KEY_USER_INFO_FMT, @event.Review.UID),
                F_S_REVIEWS);

            //多条评论时，删除一条的情况
            //redis.SetRemove(string.Format(KEY_USER_ALL_REVIEW_MOTTOS_FMT, @event.Review.UID),
            //    @event.Review.MID.ToString());
        }

        public void HandleEvent(UnloveMottoEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            redis.HashDecrement(string.Format(KEY_USER_INFO_FMT, @event.LoveMotto.UID),
                F_S_LOVED_MOTTOS);

            redis.SetRemove(string.Format(KEY_USER_ALL_LOVED_MOTTOS_FMT, @event.LoveMotto.UID),
                @event.LoveMotto.MID.ToString());
        }

        public void HandleEvent(CreateVoteEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            redis.HashIncrement(string.Format(KEY_USER_INFO_FMT, @event.UID),
                F_S_VOTES);
            
            redis.HashSet(string.Format(KEY_USER_ALL_VOTE_FMT, @event.UID),
                @event.MID.ToString(), @event.Vote);

            //if (@event.Vote.Support == 1)
            //{
            //    redis.SetAdd(string.Format(KEY_USER_ALL_SUPPORT_MOTTOS_FMT, @event.Vote.UID),
            //        @event.Vote.MID);
            //}
            //else
            //{
            //    redis.SetAdd(string.Format(KEY_USER_ALL_OPPOSE_MOTTOS_FMT, @event.Vote.UID),
            //        @event.Vote.MID);
            //}
        }

        public void HandleEvent(CreateMottoEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            redis.HashIncrement(string.Format(KEY_USER_INFO_FMT, @event.Motto.UID),
                F_S_MOTTOS);

            redis.SetAdd(string.Format(KEY_USER_ALL_MOTTOS_FMT, @event.Motto.UID),
                @event.Motto.ID.ToString());
            
        }

        public void HandleEvent(CreateCollectionEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            redis.HashIncrement(string.Format(KEY_USER_INFO_FMT, @event.Collection.UID),
                F_S_COLLECTIONS);

            redis.SetAdd(string.Format(KEY_USER_ALL_COLLECTIONS_FMT, @event.Collection.UID),
                @event.Collection.ID.ToString());
        }

        public void HandleEvent(LoadUserInfoEvent @event)
        {
            RedisHelper.HashSet(string.Format(KEY_USER_INFO_FMT, @event.UserInfo.Id),
                ConvertToHashEntries(@event.UserInfo),
                TimeSpan.FromDays(7));
        }

        public void HandleEvent(LoveCollectionEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            redis.HashIncrement(string.Format(KEY_USER_INFO_FMT, @event.LoveCollection.UID),
                F_S_LOVED_COLLECTIONS);

            redis.SetAdd(string.Format(KEY_USER_ALL_LOVED_COLLECTIONS_FMT, @event.LoveCollection.UID),
                @event.LoveCollection.CID.ToString());
        }

        public void HandleEvent(UnLoveCollectionEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            redis.HashDecrement(string.Format(KEY_USER_INFO_FMT, @event.LoveCollection.UID),
                F_S_LOVED_COLLECTIONS);

            redis.SetRemove(string.Format(KEY_USER_ALL_LOVED_COLLECTIONS_FMT, @event.LoveCollection.UID),
                @event.LoveCollection.CID.ToString());
        }

        public void HandleEvent(CollectMottoEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            redis.SetAdd(string.Format(KEY_USER_ALL_COLLECTED_MOTTOS_FMT, @event.UID),
                @event.CollectionMotto.MID.ToString());
        }

        public void HandleEvent(UnCollectMottoEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            redis.SetRemove(string.Format(KEY_USER_ALL_COLLECTED_MOTTOS_FMT, @event.UID),
                @event.CollectionMotto.MID.ToString());
        }

        public void HandleEvent(LoveUserEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();

            var tasks = new Task[] {
                batch.SetAddAsync(string.Format(KEY_USER_ALL_LOVED_USERS_FMT, @event.SUID), @event.TUID),
                batch.HashIncrementAsync(string.Format(KEY_USER_INFO_FMT, @event.SUID), F_S_FOLLOWS),
                batch.HashIncrementAsync(string.Format(KEY_USER_INFO_FMT, @event.TUID), F_S_FOLLOWERS)
            };

            batch.Execute();
            Task.WaitAll(tasks);

            string notice = JsonConvert.SerializeObject(new
            {
                TUID = @event.TUID,
                Msg = "有人喜欢了你，快去看看吧",
                Extras = new Dictionary<string, string> { {"TYPE", "2" }, { "UID", @event.SUID } }
            });

            redis.Publish(CHANNEL_GENERAL_MSG, notice);
        }

        public void HandleEvent(UnLoveUserEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();

            var tasks = new Task[] {
                batch.SetRemoveAsync(string.Format(KEY_USER_ALL_LOVED_USERS_FMT, @event.SUID), @event.TUID),
                batch.HashDecrementAsync(string.Format(KEY_USER_INFO_FMT, @event.SUID), F_S_FOLLOWS),
                batch.HashDecrementAsync(string.Format(KEY_USER_INFO_FMT, @event.TUID), F_S_FOLLOWERS)
            };

            batch.Execute();
            Task.WaitAll(tasks);

        }

        public void HandleEvent(BanUserEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();

            var tasks = new Task[] {
                batch.SetAddAsync(string.Format(KEY_USER_ALL_BAN_USERS_FMT, @event.SUID), @event.TUID),
                batch.HashIncrementAsync(string.Format(KEY_USER_INFO_FMT, @event.SUID), F_S_BANS)
            };

            batch.Execute();
            Task.WaitAll(tasks);
        }

        public void HandleEvent(UnBanUserEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();
            var tasks = new Task[] {
                batch.SetRemoveAsync(string.Format(KEY_USER_ALL_BAN_USERS_FMT, @event.SUID), @event.TUID),
                batch.HashDecrementAsync(string.Format(KEY_USER_INFO_FMT, @event.SUID), F_S_BANS)
            };

            batch.Execute();
            Task.WaitAll(tasks);
        }

        private static HashEntry[] ConvertToHashEntries(User user)
        {
            //注意：方法GetUserById(UID)会依信息项数判断用户信息是否完整，增减用户缓存信息项时请调整对应判别逻辑
            HashEntry[] entries = new HashEntry[16];
            entries[0] = new HashEntry(F_DISPLAY_NAME,user.DisplayName);
            entries[1] = new HashEntry(F_THUMB, user.Thumb);
            entries[2] = new HashEntry(F_USER_NAME, user.UserName);
            entries[3] = new HashEntry(F_S_BALANCE, user.Statistics.Balance);
            entries[4] = new HashEntry(F_S_BANS, user.Statistics.Bans);
            entries[5] = new HashEntry(F_S_COLLECTIONS, user.Statistics.Collections);
            entries[6] = new HashEntry(F_S_FOLLOWERS, user.Statistics.Followers);
            entries[7] = new HashEntry(F_S_FOLLOWS, user.Statistics.Follows);
            entries[8] = new HashEntry(F_S_LOVED_COLLECTIONS, user.Statistics.LovedCollections);
            entries[9] = new HashEntry(F_S_LOVED_MOTTOS, user.Statistics.LovedMottos);
            entries[10] = new HashEntry(F_S_MOTTOS, user.Statistics.Mottos);
            entries[11] = new HashEntry(F_S_RECRUITS, user.Statistics.Recruits);
            entries[12] = new HashEntry(F_S_REVENUE, user.Statistics.Revenue);
            entries[13] = new HashEntry(F_S_REVIEWS, user.Statistics.Reviews);
            entries[14] = new HashEntry(F_S_VOTES, user.Statistics.Votes);
            entries[15] = new HashEntry(F_SEX, user.Sex);

            return entries;
        }

        private static User ConvertFromHashEntries(HashEntry[] entries)
        {
            var user = new User();
            user.Statistics = new StatisticsViaUser();
            foreach (var item in entries)
            {
                switch (item.Name)
                {
                    case F_USER_NAME: user.UserName = item.Value; break;
                    case F_THUMB: user.Thumb = item.Value; break;
                    case F_SEX: user.Sex = int.Parse(item.Value); break;
                    case F_DISPLAY_NAME: user.DisplayName = item.Value; break;
                    case F_S_BALANCE: user.Statistics.Balance = int.Parse(item.Value); break;
                    case F_S_BANS: user.Statistics.Bans = int.Parse(item.Value); break;
                    case F_S_COLLECTIONS: user.Statistics.Collections = int.Parse(item.Value); break;
                    case F_S_FOLLOWERS: user.Statistics.Followers = int.Parse(item.Value); break;
                    case F_S_FOLLOWS: user.Statistics.Follows = int.Parse(item.Value); break;
                    case F_S_LOVED_COLLECTIONS: user.Statistics.LovedCollections = int.Parse(item.Value); break;
                    case F_S_LOVED_MOTTOS: user.Statistics.LovedMottos = int.Parse(item.Value); break;
                    case F_S_MOTTOS: user.Statistics.Mottos = int.Parse(item.Value); break;
                    case F_S_RECRUITS: user.Statistics.Recruits = int.Parse(item.Value); break;
                    case F_S_REVENUE: user.Statistics.Revenue = int.Parse(item.Value); break;
                    case F_S_REVIEWS: user.Statistics.Reviews = int.Parse(item.Value); break;
                    case F_S_VOTES: user.Statistics.Votes = int.Parse(item.Value); break;
                    default: break;
                }
            }

            return user;
        }

        public List<long> GetUserCollectionIds(string uid, int pIndex, int pSize)
        {
            var redis = RedisHelper.GetDatabase();
            var cids = redis.ListRange(string.Format(KEY_USER_ALL_COLLECTIONS_FMT, uid), (pIndex - 1) * pSize, pIndex * pSize - 1);

            return cids.Select(d => (long)d).ToList();
        }

        public async Task<List<long>> GetCollectedMottoIds(string uid, IEnumerable<long> mids)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();

            var tasks = new List<Task>();
            var collected = new List<long>();
            var key = string.Format(KEY_USER_ALL_COLLECTED_MOTTOS_FMT, uid);

            foreach (var item in mids)
            {
                var task = batch.SetContainsAsync(key, item).ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        collected.Add(item);
                    }
                });

                tasks.Add(task);
            }

            batch.Execute();

            await Task.WhenAll(tasks);

            return collected;
        }

        public async Task<List<long>> GetLovedMottoIds(string uid, IEnumerable<long> mids)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();
            var tasks = new List<Task>();
            var loved = new List<long>();
            var key = string.Format(KEY_USER_ALL_LOVED_MOTTOS_FMT, uid);

            foreach (var item in mids)
            {
                var task = batch.SetContainsAsync(key, item).ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        loved.Add(item);
                    }
                });

                tasks.Add(task);
            }

            batch.Execute();

            await Task.WhenAll(tasks);

            return loved;

        }

        public async Task<Dictionary<long, int>> GetVotedStates(string uid, IEnumerable<long> mids)
        {
            Dictionary<long, int> result = new Dictionary<long, int>();
            var redis = RedisHelper.GetDatabase();
            
            var batch = redis.CreateBatch();
            var tasks = new List<Task>();
            var suppoted = new List<long>();

            var key = string.Format(KEY_USER_ALL_VOTE_FMT, uid);

            foreach (var item in mids)
            {
                var task = batch.HashGetAsync(key, item.ToString()).ContinueWith(t => {
                    if (t.Result.HasValue)
                    {
                        result.Add(item, (int)t.Result);
                    }
                    else {
                        result.Add(item, 9); //未投
                    }
                });

                tasks.Add(task);
            }

            batch.Execute();

            await Task.WhenAll(tasks);

            return result;
        }

        public bool HasLovedMotto(string uid, long mid)
        {
            var key = string.Format(KEY_USER_ALL_LOVED_MOTTOS_FMT, uid);
            return RedisHelper.SetContains(key, mid.ToString());
        }

        public bool HasReviewed(string uid, long mid)
        {
            var key = string.Format(KEY_USER_ALL_REVIEW_MOTTOS_FMT, uid);
            return RedisHelper.SetContains(key, mid.ToString());
        }

        public bool HasCollectedMotto(string uid, long mid)
        {
            var key = string.Format(KEY_USER_ALL_COLLECTED_MOTTOS_FMT, uid);
            return RedisHelper.SetContains(key, mid.ToString());
        }

        //返回投票状态 NotYet = 9, Supported = 1, Opposed = -1， Middle = 0
        public int HasVoted(string uid, long mid)
        {
            var redis = RedisHelper.GetDatabase();
            var val = redis.HashGet(string.Format(KEY_USER_ALL_VOTE_FMT, uid), mid.ToString());

            if (val.HasValue) {
                return (int)val;
            }

            return 9;
        }

        public async Task<List<long>> GetReviewedMottoIds(string uid, IEnumerable<long> mids)
        {
            var redis = RedisHelper.GetDatabase();
            var result = new List<long>();
            var key = string.Format(KEY_USER_ALL_REVIEW_MOTTOS_FMT, uid);

            if (mids.Count() > 1)
            {
                var tasks = new List<Task>();
                var batch = redis.CreateBatch();

                foreach (var mid in mids)
                {
                    var task = batch.SetContainsAsync(key, mid.ToString()).ContinueWith(tresult =>
                    {
                        if (tresult.Result)
                        {
                            result.Add(mid);
                        }
                    });

                    tasks.Add(task);
                }

                batch.Execute();

                await Task.WhenAll(tasks);
            }
            else
            {
                var contains = redis.SetContains(key, mids.First().ToString());
                if (contains) {
                    result.Add(mids.First());
                }
            }

            return result;
        }

        public async Task<List<long>> GetLovedCollectionIds(string uid, IEnumerable<long> cids)
        {
            var redis = RedisHelper.GetDatabase();
            var result = new List<long>();

            if (cids.Count() > 1)
            {
                var key = string.Format(KEY_USER_ALL_LOVED_COLLECTIONS_FMT, uid);
                var tasks = new List<Task>();
                var batch = redis.CreateBatch();
                
                foreach (var cid in cids)
                {
                    var task = batch.SetContainsAsync(key, cid).ContinueWith(tresult =>
                    {
                        if (tresult.Result)
                        {
                            result.Add(cid);
                        }
                    });

                    tasks.Add(task);
                }

                batch.Execute();

                await Task.WhenAll(tasks);
            }
            else
            {
                var contains = redis.SetContains(string.Format(KEY_USER_ALL_LOVED_COLLECTIONS_FMT, uid),
                    cids.First().ToString());
                if (contains)
                {
                    result.Add(cids.First());
                }
            }

            return result;
        }


        //public async Task<Tuple<long?, long?>> GetUserRank(string uid, int theday, int yesterday)
        //{
        //    var redis = RedisHelper.GetDatabase();
        //    var batch = redis.CreateBatch();
        //    var rankTask = batch.SortedSetRankAsync(string.Format(KEY_USER_RANK_FMT, theday), uid, Order.Descending);
        //    var lastRankTask = batch.SortedSetRankAsync(string.Format(KEY_USER_RANK_FMT, yesterday), uid, Order.Descending);
            
        //    batch.Execute();

        //    var rank = await rankTask;
        //    var lastRank = await lastRankTask;

        //    return Tuple.Create(rank, lastRank);
        //}

        //public List<UserScore> GetUserRange(int theday, int yesterday, int start, int stop)
        //{
        //    var redis = RedisHelper.GetDatabase();
        //    var key = string.Format(KEY_USER_RANK_FMT, theday);
        //    List<UserScore> result = null;

        //    if (redis.KeyExists(key))
        //    {
        //        var entries = redis.SortedSetRangeByRankWithScores(key,
        //            start, stop, Order.Descending);

        //        result = entries.Select(entry => new UserScore
        //        {
        //            UID = entry.Element,
        //            Score = (long)entry.Score
        //        }).ToList();
        //    }
        //    else
        //    {
        //        key = string.Format(KEY_USER_RANK_FMT, yesterday);
        //        var entries = redis.SortedSetRangeByRankWithScores(key,
        //           start, stop, Order.Descending);

        //        result = entries.Select(entry => new UserScore
        //        {
        //            UID = entry.Element,
        //            Score = (long)entry.Score
        //        }).ToList();
        //    }

        //    return result;
        //}

        //public async Task TryFillUserInfo(List<UserScore> users)
        //{
        //    var redis = RedisHelper.GetDatabase();

        //    var tasks = new List<Task>();
        //    var batch = redis.CreateBatch();

        //    foreach (var item in users)
        //    {
        //        var key = string.Format(KEY_USER_INFO_FMT, item.UID);

        //        var task = batch.HashGetAsync(key, F_DISPLAY_NAME).ContinueWith(tresult =>
        //        {   
        //            item.Name = tresult.Result;
        //        });

        //        var task2 = batch.HashGetAsync(key, F_THUMB).ContinueWith(tresult =>
        //        {
        //            item.Thumb = tresult.Result;
        //        });

        //        tasks.Add(task);
        //        tasks.Add(task2);
        //    }

        //    batch.Execute();

        //    await Task.WhenAll(tasks);
        //}

        public async Task<UserRelation> GetRelation(string userId, string targetUId)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();
            var result = new UserRelation();
            var tasks = new List<Task>();
            tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_LOVED_USERS_FMT, userId), targetUId).ContinueWith(tr =>
            {
                result.SLoveT = tr.Result;
            }));

            tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_LOVED_USERS_FMT, targetUId), userId).ContinueWith(tr =>
            {
                result.TLoveS = tr.Result;
            }));

            tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_BAN_USERS_FMT, userId), targetUId).ContinueWith(tr =>
            {
                result.SBanT = tr.Result;
            }));

            batch.Execute();

            await Task.WhenAll(tasks);

            return result;
        }

        public async Task<Dictionary<string, UserRelation>> GetRelations(string suid, IEnumerable<string> tuids)
        {
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();
            var results = new Dictionary<string, UserRelation>();
            var tasks = new List<Task>();

            foreach (var tuid in tuids)
            {
                var result = new UserRelation();
                results.Add(tuid, result);

                tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_LOVED_USERS_FMT, suid), tuid).ContinueWith(tr =>
                {
                    result.SLoveT = tr.Result;
                }));

                tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_LOVED_USERS_FMT, tuid), suid).ContinueWith(tr =>
                {
                    result.TLoveS = tr.Result;
                }));

                tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_BAN_USERS_FMT, suid), tuid).ContinueWith(tr =>
                {
                    result.SBanT = tr.Result;
                }));
            }

            batch.Execute();

            await Task.WhenAll(tasks);

            return results;
        }

        public async Task<Dictionary<string, bool>> HasBans(string suid, IEnumerable<string> tuids)
        {
            var results = new Dictionary<string, bool>();
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();
            var tasks = new List<Task>();

            foreach (var tuid in tuids)
            {
                tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_BAN_USERS_FMT, suid), tuid).ContinueWith(tr =>
                {
                    results[tuid] = tr.Result;
                }));
            }
            batch.Execute();

            await Task.WhenAll(tasks);

            return results;
        }

        public async Task<Dictionary<string, bool>> IsFollowers(string suid, IEnumerable<string> tuids)
        {
            var results = new Dictionary<string, bool>();
            var redis = RedisHelper.GetDatabase();
            var batch = redis.CreateBatch();
            var tasks = new List<Task>();
            foreach (var tuid in tuids)
            {
                tasks.Add(batch.SetContainsAsync(string.Format(KEY_USER_ALL_LOVED_USERS_FMT, tuid), suid).ContinueWith(tr =>
                {
                    results[tuid] = tr.Result;
                }));
            }

            batch.Execute();

            await Task.WhenAll(tasks);

            return results;
        }

        public void HandleEvent(UpdateUserNameEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            var entries = new HashEntry[] 
            {
                new HashEntry(F_DISPLAY_NAME, @event.UserName),
                new HashEntry(F_USER_NAME, @event.UserName)
            };

            redis.HashSet(string.Format(KEY_USER_INFO_FMT, @event.UID),
               entries);
            
        }

        public void HandleEvent(UpdateUserThumbEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            redis.HashSet(string.Format(KEY_USER_INFO_FMT, @event.UID),
                F_THUMB, @event.Thumb);
        }

        public void HandleEvent(UpdateSexEvent @event)
        {
            var redis = RedisHelper.GetDatabase();

            redis.HashSet(string.Format(KEY_USER_INFO_FMT, @event.UID),
                F_SEX, @event.Sex.ToString());
        }

        public void HandleEvent(SendPrivateMsgEvent @event)
        {
            var user = GetUserById(@event.SUID);

            string notice = JsonConvert.SerializeObject(new
            {
                TUID = @event.TUID,
                Msg = "收到一条私信，快去看看吧",
                Extras = new Dictionary<string, string> { { "TYPE", "1" }, { "UID", @event.SUID } }
            });
            
            RedisHelper.GetDatabase().Publish(CHANNEL_GENERAL_MSG, notice);
        }
    }
}
