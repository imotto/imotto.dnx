using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadGiftsHandler : BaseHandler<ReadGiftsRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public ReadGiftsHandler(IGiftRepo giftRepo) 
        {
            _giftRepo = giftRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadGiftsRequest reqObj)
        {
            var result = await _giftRepo.GetGiftsAsync(reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<Gift>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = result
            };
        }
    }
}
