using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class AddRelAccountHandler : BaseHandler<AddRelAccountRequest>
    {
        private readonly IUserRepo _userRepo;

        public AddRelAccountHandler(IUserRepo userRepo)
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddRelAccountRequest reqObj)
        {
            var account = new RelAccount();
            account.AccountName = reqObj.AccountName;
            account.AccountNo = reqObj.AccountNo;
            account.Platform = reqObj.Platform;
            account.UID = reqObj.UserId;

            int rowAffected = await _userRepo.AddRelAccountAsync(account);

            if (rowAffected > 0)
            {
                return new HandleResult<long>
                {
                    Data = account.ID,
                    Msg = string.Empty,
                    State = HandleStates.Success
                };
            }

            return new HandleResult
            {
                State = HandleStates.NoDataFound,
                Msg = "出了点问题，请稍后重试"
            };
        }
    }
}
