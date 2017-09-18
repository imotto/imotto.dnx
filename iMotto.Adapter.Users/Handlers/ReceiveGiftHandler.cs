using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ReceiveGiftHandler : BaseHandler<ReceiveGiftRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public ReceiveGiftHandler(IGiftRepo giftRepo)
        {
            NeedVerifyUser = true;
            _giftRepo = giftRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReceiveGiftRequest reqObj)
        {
            int rowAfftected = await _giftRepo.ReceiveGiftAsync(reqObj.UserId, reqObj.ExchangeId);
            if (rowAfftected > 0) {
                return new HandleResult {
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
