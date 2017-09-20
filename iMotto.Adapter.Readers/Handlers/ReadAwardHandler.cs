using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadAwardHandler : BaseHandler<ReadAwardsRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IGiftRepo _giftRepo;

        public ReadAwardHandler(ICacheManager cacheManager, IGiftRepo giftRepo)
        {
            _cacheManager = cacheManager;
            _giftRepo = giftRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadAwardsRequest reqObj)
        {
            string uid = null;
            var tuple = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);

            if (tuple != null)
            {
                uid = tuple.Item1;
            }

            List<Award> result = await _giftRepo.GetAwardsAsync(uid, reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<Award>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = result
            };
        }
    }
}
