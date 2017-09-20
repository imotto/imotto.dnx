using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadRelAccountsHandler : BaseHandler<ReadRelAccountsRequest>
    {
        private readonly IUserRepo _userRepo;

        public ReadRelAccountsHandler(IUserRepo userRepo)
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadRelAccountsRequest reqObj)
        {
            List<RelAccount> accounts = await _userRepo.GetUserRelAccountsAsync(reqObj.UserId);

            return new HandleResult<List<RelAccount>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = accounts
            };
        }
    }
}
