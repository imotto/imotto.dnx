using iMotto.Adapter.Users.Requests;
using iMotto.Common;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ReviewGiftHandler : BaseHandler<ReviewGiftRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public ReviewGiftHandler(IGiftRepo giftRepo) 
        {
            NeedVerifyUser = true;
            _giftRepo = giftRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReviewGiftRequest reqObj)
        {
            if (reqObj.Rate < 0 || reqObj.Rate > 5) {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "评分必须介于0和5之间"
                };
            }

            if (reqObj.Comment.Length >= 500) {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "评价内容必须在500个字以内"
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.Comment))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "评论内容中包含敏感字符，请修正"
                };
            }

            int rowAfftected = await _giftRepo.ReviewGiftAsync(reqObj.UserId, reqObj.GiftId, reqObj.ExchangeId,
                reqObj.Rate, reqObj.Comment);
            if (rowAfftected > 0)
            {
                return new HandleResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };
            }

            return new HandleResult
            {
                State = HandleStates.NoDataFound,
                Msg = "未找到兑换记录，请刷新后重试"
            };
        }
    }
}
