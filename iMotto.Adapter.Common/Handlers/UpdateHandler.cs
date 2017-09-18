using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 版本更新
    /// </summary>
    class UpdateHandler : BaseHandler<UpdateRequest>
    {
        private readonly ICommonRepo _commonRepo;

        public UpdateHandler(ICommonRepo commonRepo)
        {
            _commonRepo = commonRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(UpdateRequest reqObj)
        {
            var update = await _commonRepo.TryUpdateAsync(reqObj.Sign, reqObj.Type, reqObj.Version);

            if (update == null)
            {
                return new UpdateResult
                {
                    State = HandleStates.Success,
                    Msg = "已是最新版本.",
                    CurVer = reqObj.Version.ToString(),
                    Url = "",
                    UpdateFlag = "A"
                };
            }

            return new UpdateResult
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                CurVer = update.DisplayVersion,
                Url = update.Url,
                ReleaseTime = update.PubDate,
                UpdateFlag = update.ForceUpdate ? "C" : "B",
                UpdateDes = update.PubDesc
            };
        }
    }
}
