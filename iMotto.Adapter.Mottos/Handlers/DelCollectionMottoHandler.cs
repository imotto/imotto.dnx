using System.Threading.Tasks;
using iMotto.Adapter.Mottos.Requests;
using iMotto.Data;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data.Entities;
using iMotto.Events;

namespace iMotto.Adapter.Mottos.Handlers
{
    class DelCollectionMottoHandler:BaseHandler<DelCollectionMottoRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICollectionRepo _collectionRepo;

        public DelCollectionMottoHandler(IEventPublisher eventPublisher, ICollectionRepo collectionRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(DelCollectionMottoRequest reqObj)
        {
            var tm = new CollectionMotto {
                CID=reqObj.CID,
                MID=reqObj.MID
            };

            var rowAffected = await _collectionRepo.RemoveCollectionMottoAsync(tm);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new UnCollectMottoEvent
                {
                    UID = reqObj.UserId,
                    CollectionMotto = tm
                });

                return new DelCollectionMottoResult {
                    State=HandleStates.Success,
                    Msg="操作成功"
                };
            }

            return new DelCollectionMottoResult {
                State=HandleStates.NoDataFound,
                Msg="未收录或不存在此偶得"
            };
            
        }
    }
}
