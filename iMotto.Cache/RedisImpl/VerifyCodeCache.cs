using System;
using iMotto.Events;
using iMotto.Data.Entities.Models;

namespace iMotto.Cache.RedisImpl
{
    class VerifyCodeCache : IVerifyCodeCache
    {
        private const string KEY_VCODE_FMT = "VCO{0}";
        private readonly RedisHelper _redisHelper;

        public VerifyCodeCache(RedisHelper redisHelper)
        {
            _redisHelper = redisHelper;
        }

        public void HandleEvent(SendVerifyCodeEvent @event)
        {
            _redisHelper.StringSet(string.Format(KEY_VCODE_FMT, @event.VerifyCode.Mobile),
                @event.VerifyCode.Code,
                TimeSpan.FromMinutes(15));
        }

        public VerifyCode PeekVerifyCodeViaMobile(string mobile)
        {
            var code = _redisHelper.StringGet(String.Format(KEY_VCODE_FMT, mobile));
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            return new VerifyCode { Mobile = mobile, Code = code };
        }

        public VerifyCode PopVerifyCodeViaMobile(string mobile)
        {
            var key = String.Format(KEY_VCODE_FMT, mobile);
            var code = _redisHelper.StringGet(key);
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            //使验证码失效
            _redisHelper.KeyDelete(key);

            return new VerifyCode { Mobile = mobile, Code = code };
        }
    }
}
