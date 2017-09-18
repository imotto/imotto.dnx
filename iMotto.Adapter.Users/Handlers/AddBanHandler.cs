using iMotto.Adapter.Users.Requests;
using iMotto.Adapter.Users.Results;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class AddBanHandler:BaseHandler<AddBanRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IBanRepo _banRepo;

        public AddBanHandler(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IBanRepo banRepo)
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _banRepo = banRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddBanRequest reqObj)
        {
            var relation = await _cacheManager.GetCache<IUserInfoCache>().GetRelation(reqObj.UserId, reqObj.TargetUId);

            if (relation.SBanT)
            {
                return new AddBanResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };
            }

            var ban = new Ban {
                BanTime=DateTime.Now,
                SUID=reqObj.UserId,
                TUID=reqObj.TargetUId
            };

            var rowAffected = await _banRepo.AddBanAsync(ban, relation.SLoveT, 
                relation.SLoveT && relation.TLoveS);

            if (rowAffected > 0)
            {
                if (relation.SLoveT)
                {
                    _eventPublisher.Publish(new UnLoveUserEvent {
                        SUID = reqObj.UserId,
                        TUID = reqObj.TargetUId
                    });
                }

                _eventPublisher.Publish(new BanUserEvent
                {
                    SUID = reqObj.UserId,
                    TUID = reqObj.TargetUId
                });

                return new AddBanResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };
            }

            return new AddBanResult
            {
                State = HandleStates.NoDataFound,
                Msg = "未找到数据"
            };

        }
    }
}
