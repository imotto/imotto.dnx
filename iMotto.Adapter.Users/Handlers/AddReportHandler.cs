using iMotto.Adapter.Users.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class AddReportHandler : BaseHandler<AddReportRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IReportRepo _reportRepo;

        public AddReportHandler(ICacheManager cacheManager, IReportRepo reportRepo) 
        {
            _cacheManager = cacheManager;
            _reportRepo = reportRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(AddReportRequest reqObj)
        {
            var uid = string.Empty;

            var tmp = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
            if (tmp != null)
            {
                uid = tmp.Item1;
            }

            var report = new Report
            {
                UID = uid,
                Type = reqObj.Type,
                TargetID = reqObj.TargetID,
                Reason = reqObj.Reason,
                ProcessTime = DateTime.Now,
                ProcessUID = string.Empty,
                Result = string.Empty,
                ReportTime = DateTime.Now,
                Status = 0
            };

            var rowAffected = await _reportRepo.AddReportAsync(report);

            return new HandleResult
            {
                State = HandleStates.Success,
                Msg = string.Empty

            };
        }
    }
}
