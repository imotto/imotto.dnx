using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadBillRecordHandler : BaseHandler<ReadBillRecordRequest>
    {
        private readonly IUserRepo _userRepo;

        public ReadBillRecordHandler(IUserRepo userRepo) 
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadBillRecordRequest reqObj)
        {
            var data = await _userRepo.GetUserBillRecordsAsync(reqObj.UserId, reqObj.PIndex, reqObj.PSize);


            return new HandleResult<List<BillRecord>>
            {
               State = HandleStates.Success,
               Msg="",
               Data = data
            };
        }
    }
}
