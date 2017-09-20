using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadMottoReviewsHandler:BaseHandler<ReadMottoReviewsRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IMottoRepo _mottoRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadMottoReviewsHandler(ICacheManager cacheManager, IMottoRepo mottoRepo, ModelBuilder modelBuilder) 
        {
            _cacheManager = cacheManager;
            _mottoRepo = mottoRepo;
            _modelBuilder = modelBuilder;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadMottoReviewsRequest reqObj)
        {
            List<Review> reviews = await _mottoRepo.GetReviewsByMottoId(reqObj.MID, reqObj.PIndex, reqObj.PSize);
            

            List<long> votedReviewIds = null;

            if (reviews != null && reviews.Count > 0)
            {
                var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                if (uInfo != null)
                {
                    votedReviewIds = await _mottoRepo.GetVotedReviewIdsByUserAndMottoIdAsync(uInfo.Item1, reqObj.MID);
                }
            }

            List<ReviewModel> result = _modelBuilder.BuildReviewModels(reviews, votedReviewIds);

            return new HandleResult<List<ReviewModel>>
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Data = result
            };
        }
    }
}
