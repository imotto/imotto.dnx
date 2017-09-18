using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class SetDefaultAddressHandler : BaseHandler<SetDefaultAddressRequest>
    {
        private readonly IUserRepo _userRepo;

        public SetDefaultAddressHandler(IUserRepo userRepo)
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(SetDefaultAddressRequest reqObj)
        {
            int rowAffected = await _userRepo.SetDefaultAddressAsync(reqObj.UserId, reqObj.AddrId);
            
            if (rowAffected > 0)
            {
                return new HandleResult
                {
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
