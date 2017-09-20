using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadAddressesHandler : BaseHandler<ReadAddressesRequest>
    {
        private readonly IUserRepo _userRepo;

        public ReadAddressesHandler(IUserRepo userRepo)
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadAddressesRequest reqObj)
        {
            List<UserAddress> addrs = await _userRepo.GetUserAddressesAsync(reqObj.UserId);

            return new HandleResult<List<UserAddress>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = addrs
            };
        }
    }
}
