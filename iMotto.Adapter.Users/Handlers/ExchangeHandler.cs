using iMotto.Adapter.Users.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ExchangeHandler : BaseHandler<ExchangeRequest>
    {
        private const string SYNC_TYPE_EXCHANGE = "EXCHANGE";
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;
        private readonly IGiftRepo _giftRepo;

        public ExchangeHandler(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IUserRepo userRepo, 
            IGiftRepo giftRepo)
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
            _giftRepo = giftRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ExchangeRequest reqObj)
        {
            bool syncLocked = false;
            int totalWait = 0;
            
            do
            {
                if (totalWait>10) {
                    return new HandleResult {
                        State = HandleStates.TimeOut,
                        Msg = "排队人数过多，请稍候再试。"
                    };
                }
                await Task.Delay(1000);
                syncLocked = _cacheManager.GetCache<ISyncRootCache>().AcquireSyncLock(SYNC_TYPE_EXCHANGE, reqObj.GiftId);
                totalWait++;
            } while (!syncLocked);
            
            ExchangeResult exchangeResult = await _giftRepo.DoExchangeAsync(reqObj.UserId, reqObj.GiftId, reqObj.ReqInfoId, reqObj.Amount);

            HandleResult result = null;

            switch (exchangeResult)
            {
                case ExchangeResult.Success:

                    ReloadUserInfo(reqObj.UserId);

                    result = new HandleResult
                    {
                        State = HandleStates.Success,
                        Msg = "礼品兑换成功"
                    };
                    break;
                case ExchangeResult.InsufficientBalance:
                    result = new HandleResult {
                        State = HandleStates.InvalidData,
                        Msg = "可用金币不足"
                    };
                    break;
                case ExchangeResult.ReachLimit:
                    result = new HandleResult {
                        State = HandleStates.InvalidData,
                        Msg = "已经兑换过此礼品",
                    };
                    break;
                case ExchangeResult.SoldOut:
                    result = new HandleResult
                    {
                        State = HandleStates.NoDataFound,
                        Msg = "礼品被兑换完了，下次早点来吧"
                    };
                    break;
                case ExchangeResult.ConcurrencyError:
                    result = new HandleResult
                    {
                        State = HandleStates.InvalidData,
                        Msg = "出错了，请稍候重试"
                    };
                    break;
                case ExchangeResult.RequireInfo:
                    result = new HandleResult {
                        State = HandleStates.InvalidData,
                        Msg="礼品接收信息有误，请重试"
                    };
                    break;
                default:
                    result = new HandleResult {
                        State = HandleStates.UnkownError,
                        Msg = "出了点问题，请稍后重试"
                    };
                    break;
            }

            return result;
        }

        /// <summary>
        /// 更新缓存中的用户信息
        /// </summary>
        /// <param name="userId"></param>
        private void ReloadUserInfo(string userId)
        {
            var user = _userRepo.GetUserById(userId);

            if (user != null) {
                _eventPublisher.Publish(new LoadUserInfoEvent {
                    UserInfo = user
                });
            }
        }
    }
}
