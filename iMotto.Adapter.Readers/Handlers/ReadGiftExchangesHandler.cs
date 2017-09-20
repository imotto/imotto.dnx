using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadGiftExchangesHandler : BaseHandler<ReadGiftExchangesRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public ReadGiftExchangesHandler(IGiftRepo giftRepo) 
        {
            _giftRepo = giftRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadGiftExchangesRequest reqObj)
        {
            var result = await _giftRepo.GetGiftExchangesAsync(reqObj.GiftId, reqObj.PIndex, reqObj.PSize);
            
            return new HandleResult<List<GiftExchange>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = result
            };
        }
    }
}
