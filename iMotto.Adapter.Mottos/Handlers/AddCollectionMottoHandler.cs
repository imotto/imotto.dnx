using System;
using System.Threading.Tasks;
using iMotto.Adapter.Mottos.Requests;
using iMotto.Data;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data.Entities;
using iMotto.Events;

namespace iMotto.Adapter.Mottos.Handlers
{
    class AddCollectionMottoHandler:BaseHandler<AddCollectionMottoRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICollectionRepo _collectionRepo;

        public AddCollectionMottoHandler(IEventPublisher eventPublisher, ICollectionRepo collectionRepo)
        {

            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddCollectionMottoRequest reqObj)
        {
            var tm = new CollectionMotto {
                CID=reqObj.CID,
                MID=reqObj.MID,
                AddTime=DateTime.Now
            };

            var rowAffected = await _collectionRepo.AddCollectionMottoAsync(tm);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new CollectMottoEvent
                {
                    UID = reqObj.UserId,
                    CollectionMotto = tm
                });


                return new AddCollectionMottoResult {
                    State=HandleStates.Success,
                    Msg="操作成功"
                };
            }

            return new AddCollectionMottoResult {
                State=HandleStates.UnkownError,
                Msg="未知错误"
            };
        }
    }
}
