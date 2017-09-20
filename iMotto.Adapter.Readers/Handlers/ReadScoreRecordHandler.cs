using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadScoreRecordHandler : BaseHandler<ReadScoreRecordRequest>
    {
        private readonly IUserRepo _userRepo;

        public ReadScoreRecordHandler(IUserRepo userRepo) 
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadScoreRecordRequest reqObj)
        {
            var data = await _userRepo.GetUserScoreRecordAsync(reqObj.UserId, reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<ScoreRecord>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = data
            };
        }
    }
}
