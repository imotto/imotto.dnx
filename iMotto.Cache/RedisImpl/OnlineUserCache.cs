using iMotto.Events;
using System;

namespace iMotto.Cache.RedisImpl
{
    class OnlineUserCache : IOnlineUserCache
    {
        private const string ONLINE_USER_SIGN_KEY_FMT = "OLU{0}";
        private const string USER_ROLE_KEY_FMT = "UIR{0:32N}{1}";
        private readonly RedisHelper _redisHelper;

        public OnlineUserCache(RedisHelper redisHelper)
        {
            _redisHelper = redisHelper;
        }


        public Tuple<string, string> GetOnlineUserViaSignature(string sign)
        {
            var wrapper = _redisHelper.StringGet(string.Format(ONLINE_USER_SIGN_KEY_FMT, sign));

            if (!string.IsNullOrWhiteSpace(wrapper) && wrapper.Length == 64)
            {
                return Tuple.Create(wrapper.Substring(0, 32),
                    wrapper.Substring(32));
            }

            return null;
        }

        public bool HasUserInRole(string userId, string role)
        {
            var redis = _redisHelper.GetDatabase();
            var result = (string)redis.StringGet(string.Format(USER_ROLE_KEY_FMT, userId, role));

            return "YES".Equals(result);
        }


        public void HandleEvent(UserLoginEvent @event)
        {
            _redisHelper.StringSet(string.Format(ONLINE_USER_SIGN_KEY_FMT, @event.Signature),
                string.Format("{0}{1}", @event.UserId, @event.Token),TimeSpan.FromDays(31));
        }

       
        public void Logout(string sign)
        {
            _redisHelper.KeyDelete(string.Format(ONLINE_USER_SIGN_KEY_FMT, sign));
        }
    }
}
