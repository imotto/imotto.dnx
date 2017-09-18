using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class SetAwardAddressHandler : BaseHandler<SetAwardAddressRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public SetAwardAddressHandler(IGiftRepo giftRepo)
        {
            NeedVerifyUser = true;
            _giftRepo = giftRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(SetAwardAddressRequest reqObj)
        {
            int result = await _giftRepo.SetAwardAddressAsync(reqObj.AwardId, reqObj.UserId, reqObj.AddrId);

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
