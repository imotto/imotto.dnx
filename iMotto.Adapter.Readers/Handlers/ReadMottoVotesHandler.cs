using System.Collections.Generic;
using System.Threading.Tasks;
using iMotto.Adapter.Readers.Requests;
using iMotto.Data.Entities;
using iMotto.Data;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadMottoVotesHandler : BaseHandler<ReadMottoVotesRequest>
    {
        private readonly IMottoRepo _mottoRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadMottoVotesHandler(IMottoRepo mottoRepo, ModelBuilder modelBuilder) 
        {
            _mottoRepo = mottoRepo;
            _modelBuilder = modelBuilder;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadMottoVotesRequest reqObj)
        {
            List<Vote> votes = await _mottoRepo.GetVotesByMottoAsync(reqObj.MID, reqObj.PIndex, reqObj.PSize);
            
            var models = _modelBuilder.BuildVoteModels(votes);

            return new HandleResult<List<VoteModel>> {
                State = HandleStates.Success,
                Data = models
            };
        }
    }
}
