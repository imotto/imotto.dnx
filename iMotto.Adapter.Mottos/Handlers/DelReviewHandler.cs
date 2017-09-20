using iMotto.Adapter.Mottos.Requests;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Handlers
{
    class DelReviewHandler : BaseHandler<DelReviewRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMottoRepo _mottoRepo;

        public DelReviewHandler(IEventPublisher eventPublisher, IMottoRepo mottoRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(DelReviewRequest reqObj)
        {
            var review = new Review
            {
                MID = reqObj.MID,
                ID = reqObj.RID,
                UID = reqObj.UserId
            };

            var rowAffected = await _mottoRepo.RemoveReviewAsync(review);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new RemoveReviewEvent
                {
                    TheDay = reqObj.TheDay,
                    Review = review
                });


                return new DelReviewResult
                {
                    State = HandleStates.Success,
                    Msg = "操作成功"
                };
            }

            return new DelReviewResult
            {
                State = HandleStates.NoDataFound,
                Msg = "评论已被删除"
            };

        }
    }
}
