using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadAwardeeHandler : BaseHandler<ReadAwardeeRequest>
    {
        private readonly IGiftRepo _giftRepo;

        public ReadAwardeeHandler(IGiftRepo giftRepo) 
        {
            _giftRepo = giftRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadAwardeeRequest reqObj)
        {
            var result = await _giftRepo.GetAwardeesAsync(reqObj.AwardId);

            return new HandleResult<List<Awardee>> {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = result
            };
        }
    }
}
