using iMotto.Adapter.Mottos.Requests;
using iMotto.Common;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Handlers
{
    class UpdateCollectionHandler : BaseHandler<UpdateCollectionRequest>
    {
        private readonly ICollectionRepo _collectionRepo;

        public UpdateCollectionHandler(ICollectionRepo collectionRepo) 
        {
            NeedVerifyUser = true;
            _collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(UpdateCollectionRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.Title))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "珍藏标题不能为空."
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.Title))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "珍藏标题中包含敏感词语，请修正"
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.Summary))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "珍藏说明中包含敏感词语，请修正"
                };
            }


            int rowAffected = await _collectionRepo.UpdateCollectionAsync(reqObj.UserId, reqObj.CID, reqObj.Title, reqObj.Summary);

            if (rowAffected > 0) {
                return new HandleResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };
            }

            return new HandleResult {
                State = HandleStates.NoDataFound,
                Msg = "未找到要修改的记录"
            };
        }
    }
}
