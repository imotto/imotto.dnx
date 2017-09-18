using iMotto.Adapter.Users.Requests;
using iMotto.Adapter.Users.Results;
using iMotto.Cache;
using iMotto.Common;
using iMotto.Data;
using iMotto.Data.Entities;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class AddUserHandler : BaseHandler<AddUserRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUserRepo _userRepo;

        public AddUserHandler(ICacheManager cacheManager,IUserRepo userRepo)
        {
            _cacheManager = cacheManager;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddUserRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.Mobile) || string.IsNullOrWhiteSpace(reqObj.VerifyCode))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "手机号码或验证码不能为空"
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

            if (string.IsNullOrWhiteSpace(reqObj.UserName) || string.IsNullOrWhiteSpace(reqObj.Password))
            {
                return new HandleResult {
                    State=HandleStates.InvalidData,
                    Msg="用户名和密码不能为空"
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.UserName))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "用户名中包含敏感词语，请修正"
                };
            }

            var user = new IdentityUser();
            user.Id = Utils.GenId();
            user.Email = "";
            user.EmailConfirmed = false;
            user.LockoutEnabled = false;
            user.LockoutEndDate = DateTime.Now;
            user.PhoneNumber = reqObj.Mobile;
            user.PhoneNumberConfirmed = true;
            user.SecurityStamp = string.Empty;
            user.TwoFactorAuthEnabled = false;
            user.UserName = reqObj.UserName;
            user.PasswordHash = Utils.HashPassword(reqObj.Password);
            user.DisplayName = string.IsNullOrWhiteSpace(reqObj.NickName) ? reqObj.UserName : reqObj.NickName;

            var rowAffected= await _userRepo.InsertAsync(user, reqObj.InviteCode);

            if (rowAffected > 0)
            {
                return new AddUserResult {
                    State=HandleStates.Success,
                    Msg=string.Empty
                };
            }

            return new AddUserResult
            {
                State = HandleStates.InvalidData,
                Msg = "发生错误"
            };
        }
    }
}
