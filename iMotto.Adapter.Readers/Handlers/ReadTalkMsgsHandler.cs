using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadTalkMsgsHandler : BaseHandler<ReadTalkMsgsRequest>
    {
        private readonly IMsgRepo _msgRepo;

        public ReadTalkMsgsHandler(IMsgRepo msgRepo)
        {
            NeedVerifyUser = true;
            _msgRepo = msgRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadTalkMsgsRequest reqObj)
        {
            List<Talk> talks = await _msgRepo.GetTalkMsgsAsync(
                reqObj.UserId, reqObj.UID, reqObj.Start, reqObj.Take);


            return new HandleResult<List<Talk>>
            {
                Msg = string.Empty,
                State = HandleStates.Success,
                Data = talks
            };
        }
    }
}
