using iMotto.Adapter.Users.Requests;
using iMotto.Cache;
using iMotto.Common;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ResetPasswordHandler : BaseHandler<ResetPasswordRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUserRepo _userRepo;

        public ResetPasswordHandler(ICacheManager cacheManager, IUserRepo userRepo)
        {
            _cacheManager = cacheManager;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ResetPasswordRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.Mobile) || string.IsNullOrWhiteSpace(reqObj.VerifyCode))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "手机号码或验证码不能为空"
                };
            }
            else if (string.IsNullOrWhiteSpace(reqObj.Password))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "密码不能为空"
                };
            }
            else
            {
                var vcode = _cacheManager.GetCache<IVerifyCodeCache>().PeekVerifyCodeViaMobile(reqObj.Mobile);

                if (vcode == null)
                {
                    return new HandleResult
                    {
                        State = HandleStates.InvalidVerifyCode,
                        Msg = "验证码无效或已过期"
                    };
                }

                if (!vcode.Code.Equals(reqObj.VerifyCode))
                {
                    return new HandleResult
                    {
                        State = HandleStates.InvalidVerifyCode,
                        Msg = "验证码不正确"
                    };
                }

            }


            var rowAffected = await _userRepo.ResetPasswordAsync(reqObj.Mobile,
                Utils.HashPassword(reqObj.Password));

            if (rowAffected > 0)
            {
                return new HandleResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };
            }

            return new HandleResult
            {
                State = HandleStates.InvalidData,
                Msg = "手机号码不存在"
            };
        }
    }
}
