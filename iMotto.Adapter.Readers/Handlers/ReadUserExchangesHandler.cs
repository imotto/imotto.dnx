using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserExchangesHandler : BaseHandler<ReadUserExchangesRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public ReadUserExchangesHandler(IGiftRepo giftRepo)
        {
            NeedVerifyUser = true;
            _giftRepo = giftRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserExchangesRequest reqObj)
        {
            var result = await _giftRepo.GetUserExchangesAsync(reqObj.UserId, reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<GiftExchange>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = result
            };
        }
    }
}
