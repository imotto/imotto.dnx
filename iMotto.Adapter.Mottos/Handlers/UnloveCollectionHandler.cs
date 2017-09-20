using iMotto.Adapter.Mottos.Requests;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Handlers
{
    class UnloveCollectionHandler:BaseHandler<UnloveCollectionRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICollectionRepo _collectionRepo;

        public UnloveCollectionHandler(IEventPublisher eventPublisher, ICollectionRepo collectionRepo) 
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(UnloveCollectionRequest reqObj)
        {
            var lt = new LoveCollection {
                CID=reqObj.CID,
                UID=reqObj.UserId
            };

            var rowAffected = await _collectionRepo.RemoveLoveCollectionAsync(lt);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new UnLoveCollectionEvent
                {
                    LoveCollection = lt
                });

                return new UnloveCollectionResult {
                    State=HandleStates.Success,
                    Msg="操作成功"
                };
            }

            return new UnloveCollectionResult {
                State=HandleStates.NoDataFound,
                Msg="珍藏不存在或已被删除"
            };
        }
    }
}
