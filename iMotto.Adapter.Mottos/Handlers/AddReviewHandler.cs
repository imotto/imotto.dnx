using iMotto.Adapter.Mottos.Requests;
using iMotto.Adapter.Mottos.Results;
using iMotto.Common;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Handlers
{
    class AddReviewHandler : BaseHandler<AddReviewRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMottoRepo _mottoRepo;

        public AddReviewHandler(IEventPublisher eventPublisher, IMottoRepo mottoRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddReviewRequest reqObj)
        {
            if (reqObj.Content.Length >= 500)
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "评论信息不能超过500个字"
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.Content))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "评论信息中包含敏感词语，请修正"
                };
            }


            var review = new Review
            {
                AddTime = DateTime.Now,
                Content = reqObj.Content,
                MID = reqObj.MID,
                UID = reqObj.UserId,
                Up = 0,
                Down = 0
            };

            var rowAffected = await _mottoRepo.AddReviewAsync(review);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new CreateReviewEvent
                {
                    TheDay = reqObj.TheDay,
                    Review = review
                });

                return new AddReviewResult
                {
                    State = HandleStates.Success,
                    Msg = ""
                };
            }

            return new AddReviewResult
            {
                State = HandleStates.NoDataFound,
                Msg = "评论的偶得已删除"
            };
        }
    }
}
