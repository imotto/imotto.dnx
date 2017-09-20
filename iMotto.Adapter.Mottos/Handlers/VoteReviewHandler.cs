using iMotto.Adapter.Mottos.Requests;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data;
using iMotto.Data.Entities;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Handlers
{
    class VoteReviewHandler : BaseHandler<VoteReviewRequest>
    {
        private readonly IMottoRepo _mottoRepo;

        public VoteReviewHandler(IMottoRepo mottoRepo)
        {
            NeedVerifyUser = true;
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(VoteReviewRequest reqObj)
        {
            var reviewVote = new ReviewVote
            {
                MID = reqObj.MID,
                ReviewID = reqObj.RID,
                Oppose = 0,
                Support = reqObj.Support,
                UID = reqObj.UserId,
                VoteTime = DateTime.Now
            };

            var rowAffected = await _mottoRepo.AddReviewVoteAsync(reviewVote);

            return new VoteReviewResult
            {
                State = HandleStates.Success,
                Msg = "操作成功"
            };

        }
    }
}
