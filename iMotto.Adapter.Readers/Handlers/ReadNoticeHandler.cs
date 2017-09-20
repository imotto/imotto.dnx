using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadNoticeHandler : BaseHandler<ReadNoticeRequest>
    {
        private readonly IMsgRepo _msgRepo;

        public ReadNoticeHandler(IMsgRepo msgRepo) 
        {
            NeedVerifyUser = true;
            _msgRepo = msgRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadNoticeRequest reqObj)
        {
            List<Notice> notices = await _msgRepo.GetNoticesAsync(reqObj.UserId, reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<Notice>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = notices
            };

        }
    }
}
