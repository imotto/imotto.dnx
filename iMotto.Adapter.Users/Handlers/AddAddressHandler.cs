using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class AddAddressHandler : BaseHandler<AddAddressRequest>
    {
        private readonly IUserRepo _userRepo;

        public AddAddressHandler(IUserRepo userRepo)
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddAddressRequest reqObj)
        {
            var addr = new UserAddress();
            addr.Address = reqObj.Address;
            addr.Province = reqObj.Province;
            addr.City = reqObj.City;
            addr.Contact = reqObj.Contact;
            addr.District = reqObj.District;
            addr.Mobile = reqObj.Mobile;
            addr.IsDefault = false;
            addr.UID = reqObj.UserId;
            addr.Zip = reqObj.Zip;

            int rowAfftected = await _userRepo.AddAddressAsync(addr);

            if (rowAfftected > 0) {
                return new HandleResult<long> {
                    Data = addr.ID,
                    Msg=string.Empty,
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
