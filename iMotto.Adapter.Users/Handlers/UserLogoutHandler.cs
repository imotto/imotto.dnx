using iMotto.Adapter.Users.Requests;
using iMotto.Cache;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class UserLogoutHandler : BaseHandler<UserLogoutRequest>
    {
        private readonly ICacheManager _cacheManager;

        public UserLogoutHandler(ICacheManager cacheManager) 
        {
            _cacheManager = cacheManager;
        }

        protected override async Task<HandleResult> HandleCoreAsync(UserLogoutRequest reqObj)
        {
            var token = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
            if (token != null && token.Item1.Equals(reqObj.UserId))
            {
                _cacheManager.GetCache<IOnlineUserCache>().Logout(reqObj.Sign);
            }

            await Task.Delay(0);

            return new HandleResult {
                State = HandleStates.Success,
                Msg="操作成功"
            };
        }
    }
}
