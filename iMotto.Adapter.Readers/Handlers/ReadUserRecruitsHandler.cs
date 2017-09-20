using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserRecruitsHandler:BaseHandler<ReadUserRecruitsRequest>
    {
        private readonly IRecruitRepo _recruitRepo;

        public ReadUserRecruitsHandler(IRecruitRepo recruitRepo)
        {
            _recruitRepo = recruitRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserRecruitsRequest reqObj)
        {
            List<Recruit> recruits = await _recruitRepo.GetRecruitsByUserAsync(
                reqObj.UserId, reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<Recruit>>
            {
                State = HandleStates.Success,
                Data = recruits
            };
        }
    }
}
