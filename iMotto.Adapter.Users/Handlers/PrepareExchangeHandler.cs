using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Threading.Tasks;
using iMotto.Events;
using iMotto.Cache;

namespace iMotto.Adapter.Users.Handlers
{
    class PrepareExchangeHandler : BaseHandler<PrepareExchangeReqeust>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public PrepareExchangeHandler(ICacheManager cacheManager, IEventPublisher eventPublisher, IUserRepo userRepo) 
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(PrepareExchangeReqeust reqObj)
        {
            PrepareExchangeResult result = new PrepareExchangeResult();
            result.ReqInfoType = reqObj.ReqInfoType;
            result.ReqInfoHint = GetHintByReqInfoType(reqObj.ReqInfoType);

            if (reqObj.ReqInfoType == 0)
            {
                var addresses = await _userRepo.GetUserAddressesAsync(reqObj.UserId);

                result.Addresses = addresses;
            }
            else {
                var relAccount = await _userRepo.GetUserRelAccountsAsync(reqObj.UserId, reqObj.ReqInfoType);
                result.Accounts = relAccount;
            }

            var user = TryLoadUser(reqObj.UserId);
            if (user != null)
            {
                result.Balance = user.Statistics.Balance;
            }

            return new HandleResult<PrepareExchangeResult>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = result
            };
        }

        private User TryLoadUser(string uid)
        {
            var user = _cacheManager.GetCache<IUserInfoCache>().GetUserById(uid);
            if (user != null)
            {
                return user;
            }

            user = _userRepo.GetUserById(uid);
            if (user != null)
            {
                _eventPublisher.Publish(new LoadUserInfoEvent
                {
                    UserInfo = user
                });
            }

            return user;
        }

        private string GetHintByReqInfoType(int reqInfoType)
        {
            string result="";

            switch (reqInfoType)
            {
                case 0:
                    result = "请提供收货人地址";
                    break;
                case 1:
                    result = "请提供微信账号";
                    break;
                case 2:
                    result = "请提供支付宝账号";
                    break;
                case 3:
                    result = "请提供淘宝账号";
                    break;
            }

            return result;
        }
    }
}
