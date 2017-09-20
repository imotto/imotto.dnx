using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers
{
    class ModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public ModelBuilder(ICacheManager cacheManager, IEventPublisher eventPublisher, IUserRepo userRepo)
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
        }

        public void DecorateUserRelatedData(List<MottoModel> mottos, List<long> lovedMottoIds, List<Vote> votes, List<long> collectedMids)
        {
            foreach (var item in mottos)
            {
                if (lovedMottoIds != null && lovedMottoIds.IndexOf(item.ID) >= 0)
                {
                    item.Loved = 1;
                }

                if (votes != null)
                {
                    var vote = votes.FirstOrDefault(v => v.MID == item.ID);
                    if (vote != null)
                    {
                        item.Vote = vote.Support - vote.Oppose;
                    }
                }

                if (collectedMids.IndexOf(item.ID) >= 0)
                {
                    item.Collected = 1;
                }
            }
        }


        internal void FillUserInfo(List<RecentTalk> talks)
        {
            foreach(var talk in talks)
            {
                var user = TryGetUser(talk.WithUID);
                if (user != null)
                {
                    talk.UserName = user.DisplayName;
                    talk.UserThumb = user.Thumb;
                }
                else
                {
                    talk.UserThumb = "";
                    talk.UserName = "佚名";
                }
            }
        }

        public async Task DecorateUserRelatedData(List<AlbumModel> albums, string uid)
        {
            if (string.IsNullOrWhiteSpace(uid) || albums.Count <= 0)
            {
                return;
            }

            var cache = _cacheManager.GetCache<IUserInfoCache>();

            var albumIds = albums.Select(a => a.ID);

            var loved = await cache.GetLovedCollectionIds(uid, albumIds);

            foreach (var item in albums)
            {
                item.Loved = loved.Contains(item.ID) ? 1 : 0;
            }

        }

        public async Task DecorateUserRelatedData(List<MottoModel> mottos, string uid, 
            bool setAllLoved = false, 
            bool setAllCollected = false, 
            bool setAllSupported = false,
            bool setAllReviewed = false)
        {
            if (string.IsNullOrWhiteSpace(uid) || mottos == null || mottos.Count <= 0)
            {
                return;
            }

            var cache = _cacheManager.GetCache<IUserInfoCache>();
            
            if (mottos.Count > 1)
            {
                var mids = mottos.Select(m => m.ID);

                List<long> collected = null;
                List<long> loved = null;
                List<long> reviewed = null;
                Dictionary<long, int> voteStates = null;
                if (!setAllCollected) collected = await cache.GetCollectedMottoIds(uid, mids);
                if (!setAllLoved) loved =  await cache.GetLovedMottoIds(uid, mids);
                if (!setAllReviewed) reviewed = await cache.GetReviewedMottoIds(uid, mids);
                if(!setAllSupported) voteStates = await cache.GetVotedStates(uid, mids);
                 
                foreach (var item in mottos)
                {
                    item.Loved = setAllLoved ? 1 : (loved.Contains(item.ID) ? 1 : 0);
                    item.Collected = setAllCollected ? 1 : (collected.Contains(item.ID) ? 1 : 0);
                    item.Vote = setAllSupported ? 1 : voteStates[item.ID];
                    item.Reviewed = setAllReviewed ? 1 : (reviewed.Contains(item.ID) ? 1 : 0);
                }
            }
            else
            {
                var motto = mottos[0];
                motto.Loved = setAllCollected ? 1 : (cache.HasLovedMotto(uid, motto.ID) ? 1 : 0);
                motto.Vote = setAllSupported ? 1 : cache.HasVoted(uid, motto.ID);
                motto.Reviewed = setAllReviewed ? 1 : (cache.HasReviewed(uid, motto.ID) ? 1 : 0);
                motto.Collected = setAllCollected ? 1 : (cache.HasCollectedMotto(uid, motto.ID) ? 1 : 0);
            }
        }


        public List<ReviewModel> BuildReviewModels(List<Review> reviews, List<long> votedReviewIds=null)
        {
            var result = new List<ReviewModel>();

            foreach (var item in reviews)
            {
                var model = new ReviewModel
                {
                    ID = item.ID,
                    AddTime = item.AddTime,
                    Content = item.Content,
                    Down = item.Down,
                    MID = item.MID,
                    UID = item.UID,
                    Up = item.Up,
                    Supported = 0
                };

                if (votedReviewIds != null && votedReviewIds.IndexOf(model.ID) >= 0)
                {
                    model.Supported = 1;
                }

                var user = TryGetUser(model.UID);
                if (user != null)
                {
                    model.UserName = user.DisplayName;
                    model.UserThumb = user.Thumb;
                }

                result.Add(model);
            }

            return result;
        }

        public List<AlbumModel> BuildAlbumModels(List<Collection> albums, string uid)
        {
            var result = new List<AlbumModel>();

            var user = TryGetUser(uid);
            foreach (var item in albums)
            {
                var model = new AlbumModel
                {
                    ID = item.ID,
                    CreateTime = item.CreateTime,
                    Description = item.Description,
                    Loves = item.Loves,
                    Mottos = item.Mottos,
                    Score = item.Score,
                    Tags = item.Tags,
                    Title = item.Title,
                    UID = item.UID
                };

                if (user != null)
                {
                    model.UserName = user.DisplayName;
                    model.UserThumb = user.Thumb;
                }
                else
                {
                    model.UserName = "佚名";
                    model.UserThumb = string.Empty;
                }

                result.Add(model);
            }

            return result;
        }

        public List<AlbumModel> BuildAlbumModels(List<Collection> albums)
        {
            var result = new List<AlbumModel>();

            foreach (var item in albums)
            {
                var model = new AlbumModel
                {
                    ID = item.ID,
                    CreateTime = item.CreateTime,
                    Description = item.Description,
                    Loves = item.Loves,
                    Mottos = item.Mottos,
                    Score = item.Score,
                    Tags = item.Tags,
                    Title = item.Title,
                    UID = item.UID
                };

                var user = TryGetUser(model.UID);
                if (user != null)
                {
                    model.UserName = user.DisplayName;
                    model.UserThumb = user.Thumb;
                }

                result.Add(model);
            }

            return result;
        }

        

        public List<MottoModel> BuildMottoModels(List<Motto> mottos)
        {
            var result = new List<MottoModel>();

            foreach (var item in mottos)
            {
                var model = new MottoModel {
                    ID = item.ID,
                    AddTime = item.AddTime,
                    Content = item.Content,
                    Down = item.Down,
                    Loves = item.Loves,
                    RecruitID = item.RecruitID,
                    RecruitTitle = item.RecruitTitle,
                    Reviews = item.Reviews,
                    Score = item.Score,
                    State = item.AddTime.AddDays(7) < DateTime.Now ? 1 : 0,                    
                    Up=item.Up,

                    UID = item.UID,
                    UserName ="",
                    UserThumb=""
                };

                //如果偶得正在评估中 投票状态默认为9(未投票)， 其它情况下默认为 0(中立), 
                //当查询用户已登录时，会自动返回对应的状态。
                model.Vote = model.State == 0 ? 9 : 0;

               
                var user = TryGetUser(model.UID);
                if (user != null)
                {
                    model.UserName = user.DisplayName;
                    model.UserThumb = user.Thumb;
                }


                result.Add(model);
            }

            return result;
        }


        internal List<VoteModel> BuildVoteModels(List<Vote> votes)
        {
            List<VoteModel> result = new List<VoteModel>();
            foreach (var item in votes)
            {
                var model = new VoteModel(item);

                var user = TryGetUser(model.UID);
                if (user != null)
                {
                    model.UserName = user.DisplayName;
                    model.UserThumb = user.Thumb;
                }
                
                result.Add(model);
            }

            return result;
        }

        public User TryGetUser(string uid)
        {
            var user = _cacheManager.GetCache<IUserInfoCache>().GetUserById(uid);
            if (user != null)
            {
                return user;
            }
            else
            {
                user = _userRepo.GetUserById(uid);
                if (user != null)
                {
                    _eventPublisher.Publish<LoadUserInfoEvent>(new LoadUserInfoEvent
                    {
                        UserInfo = user
                    });
                }

                return user;
            }
        }
    }
}
