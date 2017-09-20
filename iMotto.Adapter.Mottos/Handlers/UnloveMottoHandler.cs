using iMotto.Adapter.Mottos.Requests;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Handlers
{
    class UnloveMottoHandler:BaseHandler<UnloveMottoRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMottoRepo _mottoRepo;

        public UnloveMottoHandler(IEventPublisher eventPublisher, IMottoRepo mottoRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(UnloveMottoRequest reqObj)
        {
            var lm = new LoveMotto {
                MID=reqObj.MID,
                TheDay = reqObj.TheDay,
                UID=reqObj.UserId,
            };

            var rowAffected=await _mottoRepo.RemoveLoveMottoAsync(lm);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish<UnloveMottoEvent>(new UnloveMottoEvent
                {
                    LoveMotto = lm
                });

                return new UnloveMottoResult
                {
                    State = HandleStates.Success,
                    Msg="操作成功"
                };
            }

            return new UnloveMottoResult
            {
                State = HandleStates.NoDataFound,
                Msg="偶得不存在或已被删除"
            };
        }
    }
}
