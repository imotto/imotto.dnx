using iMotto.Adapter.Users.Requests;
using iMotto.Common;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ModifyPasswordHandler : BaseHandler<ModifyPasswordRequest>
    {
        private readonly IUserRepo _userRepo;

        public ModifyPasswordHandler(IUserRepo userRepo)
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ModifyPasswordRequest reqObj)
        {
            if (string.IsNullOrEmpty(reqObj.OldPassword) || string.IsNullOrEmpty(reqObj.NewPassword))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "密码输入不正确"
                };
            }

            var rowAffected = await _userRepo.UpdateUserPasswordAsync(reqObj.UserId,
                Utils.HashPassword(reqObj.OldPassword),
                Utils.HashPassword(reqObj.NewPassword));

            if (rowAffected > 0)
            {
                return new HandleResult
                {
                    State = HandleStates.Success,
                    Msg = "密码修改成功"
                };
            }

            return new HandleResult
            {
                State = HandleStates.InvalidPassword,
                Msg = "原密码输入有误,请核对后重试"
            };
        }
    }
}
