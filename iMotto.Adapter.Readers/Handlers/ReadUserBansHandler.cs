using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserBansHandler:BaseHandler<ReadUserBansRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUserRepo _userRepo;

        public ReadUserBansHandler(ICacheManager cacheManager, IUserRepo userRepo) 
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserBansRequest reqObj)
        {
            List<RelatedUser> users = await _userRepo.GetUserBansAsync(reqObj.UserId, reqObj.PIndex, reqObj.PSize);

            if (users.Count > 0)
            {
                var uids = users.Select(u => u.ID);


                var uInfoCache = _cacheManager.GetCache<IUserInfoCache>();

                Dictionary<string, bool> isFollowers = await uInfoCache.IsFollowers(reqObj.UserId, uids);
                
                foreach (var user in users)
                {
                    var state = RelationState.SBanT;
                    if (isFollowers[user.ID])
                    {
                        state = state | RelationState.TLoveS;
                    }

                    user.RelationState = (int)state;
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
