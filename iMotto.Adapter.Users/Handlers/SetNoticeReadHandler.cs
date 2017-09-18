using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class SetNoticeReadHandler : BaseHandler<SetNoticeReadRequest>
    {
        private readonly IMsgRepo _msgRepo;

        public SetNoticeReadHandler(IMsgRepo msgRepo)
        {
            NeedVerifyUser = true;
            _msgRepo = msgRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(SetNoticeReadRequest reqObj)
        {
            var rowAffected = await _msgRepo.SetNoticeReadAsync(reqObj.ID, reqObj.UserId);


            return new HandleResult
            {
                State = HandleStates.Success,
                Msg = string.Empty
            };
        }
    }
}
