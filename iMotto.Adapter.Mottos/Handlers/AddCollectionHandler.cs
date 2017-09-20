using System;
using System.Threading.Tasks;
using iMotto.Adapter.Mottos.Requests;
using iMotto.Data;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data.Entities;
using iMotto.Events;
using iMotto.Common;

namespace iMotto.Adapter.Mottos.Handlers
{
    class AddCollectionHandler : BaseHandler<AddTreasureRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICollectionRepo _collectionRepo;

        public AddCollectionHandler(IEventPublisher eventPublisher, ICollectionRepo collectionRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddTreasureRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.Title))
            {
                return new AddCollectionResult
                {
                    State = HandleStates.InvalidData,
                    ID = 0,
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

            var collection = new Collection
            {
                CreateTime = DateTime.Now,
                Description = reqObj.Summary ?? string.Empty,
                Loves = 0,
                Mottos = 0,
                Score = 0,
                Tags = reqObj.Tags ?? string.Empty,
                Title = reqObj.Title,
                UID = reqObj.UserId
            };

            var rowAffected = await _collectionRepo.AddCollectionAsync(collection);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new CreateCollectionEvent {
                    Collection = collection
                });

                return new AddCollectionResult{
                    State=HandleStates.Success,
                    Msg="操作成功",
                    ID=collection.ID
                };
            }

            return new AddCollectionResult
            {
                State = HandleStates.UnkownError,
                Msg = "发生未知错误",
                ID = 0
            };
        }
    }
}
