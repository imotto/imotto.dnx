using System;
using System.Threading.Tasks;
using iMotto.Adapter.Mottos.Requests;
using iMotto.Data;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data.Entities;
using iMotto.Events;
using iMotto.Cache;

namespace iMotto.Adapter.Mottos.Handlers
{
    class LoveMottoHandler : BaseHandler<LoveMottoRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMottoRepo _mottoRepo;

        public LoveMottoHandler(ICacheManager cacheManager, IEventPublisher eventPublisher, IMottoRepo mottoRepo)
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(LoveMottoRequest reqObj)
        {
            if (_cacheManager.GetCache<IUserInfoCache>().HasLovedMotto(reqObj.UserId, reqObj.MID))
            {
                return new LoveMottoResult
                {
                    State = HandleStates.Success,
                    Msg = "操作成功"
                };
            }

            var lm = new LoveMotto
            {
                MID = reqObj.MID,
                UID = reqObj.UserId,
                TheDay = reqObj.TheDay,
                LoveTime = DateTime.Now
            };

            var rowAffected = await _mottoRepo.AddLoveMottoAsync(lm);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish<LoveMottoEvent>(new LoveMottoEvent
                {
                    LoveMotto = lm
                });

                return new LoveMottoResult
                {
                    State = HandleStates.Success,
                    Msg = "操作成功"
                };
            }

            return new LoveMottoResult
            {
                State = HandleStates.NoDataFound,
                Msg = "偶得不存在或已被删除"
            };
        }
    }
}
