using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotto.Adapter.Users.Requests;
using iMotto.Data.Entities;
using iMotto.Data;
using iMotto.Adapter.Users.Results;
using iMotto.Cache;
using iMotto.Events;

namespace iMotto.Adapter.Users.Handlers
{
    class DelFollowHandler : BaseHandler<DelFollowRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IFollowRepo _followRepo;

        public DelFollowHandler(ICacheManager cacheManager, IEventPublisher eventPublisher, IFollowRepo followRepo)
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _followRepo = followRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(DelFollowRequest reqObj)
        {
            var relation = await _cacheManager.GetCache<IUserInfoCache>().GetRelation(reqObj.UserId, reqObj.TargetUId);

            if (!relation.SLoveT)
            {
                return new DelFollowResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };
            }

            var follow = new Follow
            {
                SUID = reqObj.UserId,
                TUID = reqObj.TargetUId,
                IsMutual = relation.TLoveS
            };

            var rowAffected = await _followRepo.RemoveFollowAsync(follow);

            _eventPublisher.Publish(new UnLoveUserEvent {
                SUID = reqObj.UserId,
                TUID = reqObj.TargetUId
            });
            

            return new DelFollowResult
            {
                State = HandleStates.Success,
            };
        }
    }
}
