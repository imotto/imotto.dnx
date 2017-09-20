using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserStatisticsHandler:BaseHandler<ReadUserStatisticsRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public ReadUserStatisticsHandler(ICacheManager cacheManager, IEventPublisher eventPublisher, IUserRepo userRepo) 
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserStatisticsRequest reqObj)
        {
            var userInfoCache = _cacheManager.GetCache<IUserInfoCache>();
            var user = userInfoCache.GetUserById(reqObj.UID);

            if (user == null)
            {
                user = await Task.Run(() => _userRepo.GetUserById(reqObj.UID));

                if (user != null)
                {
                    _eventPublisher.Publish<LoadUserInfoEvent>(new LoadUserInfoEvent {
                        UserInfo = user
                    });
                }
            }

            if (user != null)
            {
                //用户排名暂不作处理
                //var rank = await userInfoCache.GetUserRank(reqObj.UID,
                //    Utils.GetTheDay(DateTime.Today),
                //    Utils.GetTheDay(DateTime.Today.AddDays(-1)));

                //if (rank.Item1.HasValue)
                //{
                //    user.Rank = (int)rank.Item1.Value;

                //    if (rank.Item2.HasValue)
                //    {
                //        user.Change = (int)(rank.Item1.Value - rank.Item2.Value);
                //    }
                //}
                //else if (rank.Item2.HasValue)
                //{
                //    user.Rank = (int)rank.Item2.Value;
                //}
               

                var reqUser = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                if(reqUser != null)
                {
                    if (user.Id != reqUser.Item1)
                    {
                        var relation = await userInfoCache.GetRelation(reqUser.Item1, user.Id);

                        var relationState = RelationState.None;
                        if (relation.SBanT)
                        {
                            relationState = relationState | RelationState.SBanT;
                        }
                        if (relation.SLoveT)
                        {
                            relationState = relationState | RelationState.SLoveT;
                        }
                        if (relation.TLoveS)
                        {
                            relationState = relationState | RelationState.TLoveS;
                        }

                        user.RelationState = (int)relationState;
                    }
                }
            }

            return new HandleResult<User>
            {
                State = HandleStates.Success,
                Data = user
            };
            
            
        }
    }
}
