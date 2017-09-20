using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserFollowsHandler:BaseHandler<ReadUserFollowsRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUserRepo _userRepo;

        public ReadUserFollowsHandler(ICacheManager cacheManager, IUserRepo userRepo) 
        {
            _cacheManager = cacheManager;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserFollowsRequest reqObj)
        {
            List<RelatedUser> users = await _userRepo.GetFollowAsync(reqObj.UID, reqObj.PIndex, reqObj.PSize);

            if (users.Count > 0)
            {
                var uids = users.Select(u => u.ID);

                var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                if (uInfo != null)
                {
                    var uInfoCache = _cacheManager.GetCache<IUserInfoCache>();

                    if (uInfo.Item1 != reqObj.UID)
                    {
                        //读取别人关注的人
                        var relations = await uInfoCache.GetRelations(uInfo.Item1, uids);

                        foreach (var user in users)
                        {
                            var relation = relations[user.ID];
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
                    else
                    {
                        //读取自己关注的人
                        foreach (var user in users)
                        {
                            var mutualState = (int)(RelationState.SLoveT | RelationState.TLoveS);
                            user.RelationState = user.IsMutual ? mutualState : (int)RelationState.SLoveT;
                        }
                    }
                }
            }

            return new HandleResult<List<RelatedUser>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = users
            };
        }
    }
}
