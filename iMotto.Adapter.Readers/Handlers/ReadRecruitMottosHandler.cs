using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadRecruitMottosHandler : BaseHandler<ReadRecruitMottosRequest>
    {
        private readonly IMottoRepo _mottoRepo;

        public ReadRecruitMottosHandler(IMottoRepo mottoRepo)
        {
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadRecruitMottosRequest reqObj)
        {
            List<Motto> mottos = await _mottoRepo.GetMottosByRecruitAsync(reqObj.RID, reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<Motto>> {
                State=HandleStates.Success,
                Data=mottos
            };
        }
    }
}
