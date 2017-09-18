using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ReceiveAwardHandler : BaseHandler<ReceiveAwardRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public ReceiveAwardHandler(IGiftRepo giftRepo)
        {
            NeedVerifyUser = true;
            _giftRepo = giftRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReceiveAwardRequest reqObj)
        {
            int result = await _giftRepo.ReceiveAwardAsync(reqObj.UserId, reqObj.AwardId);

            if (result != 0)
            {
                return new HandleResult
                {
                    State = HandleStates.Success,
                    Msg = "操作成功"
                };
            }
            else
            {
                return new HandleResult
                {
                    State = HandleStates.NoDataFound,
                    Msg = "发生错误，请稍后重试"
                };
            }

        }
    }
}
