using iMotto.Adapter.Users.Requests;
using iMotto.Adapter.Users.Results;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class DelBanHandler : BaseHandler<DelBanRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IBanRepo _banRepo;

        public DelBanHandler(IEventPublisher eventPublisher, IBanRepo banRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _banRepo = banRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(DelBanRequest reqObj)
        {
            var ban = new Ban
            {
                SUID = reqObj.UserId,
                TUID = reqObj.TargetUId
            };

            var rowaffected = await _banRepo.RemoveBanAsync(ban);

            _eventPublisher.Publish(new UnBanUserEvent
            {
                SUID = reqObj.UserId,
                TUID = reqObj.TargetUId
            });

            return new DelBanResult
            {
                State = HandleStates.Success,
            };
        }
    }
}
