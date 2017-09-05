using System;
using iMotto.Events;
using iMotto.Data.Entities.Models;

namespace iMotto.Cache.RedisImpl
{
    class VerifyCodeCache : IVerifyCodeCache
    {
        private const string KEY_VCODE_FMT = "VCO{0}";

        public void HandleEvent(SendVerifyCodeEvent @event)
        {
            RedisHelper.StringSet(string.Format(KEY_VCODE_FMT, @event.VerifyCode.Mobile),
                @event.VerifyCode.Code,
                TimeSpan.FromMinutes(15));
        }

        public VerifyCode PeekVerifyCodeViaMobile(string mobile)
        {
            var code = RedisHelper.StringGet(String.Format(KEY_VCODE_FMT, mobile));
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            return new VerifyCode { Mobile = mobile, Code = code };
        }

        public VerifyCode PopVerifyCodeViaMobile(string mobile)
        {
            var key = String.Format(KEY_VCODE_FMT, mobile);
            var code = RedisHelper.StringGet(key);
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            //使验证码失效
            RedisHelper.KeyDelete(key);

            return new VerifyCode { Mobile = mobile, Code = code };
        }
    }
}
