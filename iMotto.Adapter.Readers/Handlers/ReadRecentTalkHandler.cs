using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadRecentTalkHandler : BaseHandler<ReadRecentTalkRequest>
    {
        private readonly IMsgRepo _msgRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadRecentTalkHandler(IMsgRepo msgRepo, ModelBuilder modelBuilder)
        {
            NeedVerifyUser = true;
            _msgRepo = msgRepo;
            _modelBuilder = modelBuilder;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadRecentTalkRequest reqObj)
        {
            List<RecentTalk> talks = await _msgRepo.GetRecentTalksAsync(
                reqObj.UserId, reqObj.PIndex, reqObj.PSize);

            _modelBuilder.FillUserInfo(talks);

            return new HandleResult<List<RecentTalk>>
            {
                Msg = string.Empty,
                State = HandleStates.Success,
                Data = talks
            };

        }
    }
}
