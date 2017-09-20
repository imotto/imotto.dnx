using System;
using System.Threading.Tasks;
using iMotto.Adapter.Mottos.Requests;
using iMotto.Data;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data.Entities;
using iMotto.Events;

namespace iMotto.Adapter.Mottos.Handlers
{
    class LoveCollectionHandler:BaseHandler<LoveCollectionRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICollectionRepo _collectionRepo;

        public LoveCollectionHandler(IEventPublisher eventPublisher, ICollectionRepo collectionRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(LoveCollectionRequest reqObj)
        {
            var lt = new LoveCollection {
                UID = reqObj.UserId,
                LoveTime = DateTime.Now,
                CID = reqObj.CID,
                CollectionTitle=""
            };

            var rowAffected = await _collectionRepo.AddLoveCollectionAsync(lt);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish<LoveCollectionEvent>(new LoveCollectionEvent {
                    LoveCollection = lt
                });

                return new LoveCollectionResult {
                    State=HandleStates.Success,
                    Msg="操作成功"
                };
            }

            return new LoveCollectionResult
            {
                State=HandleStates.NoDataFound,
                Msg="珍藏不存在"
            };
        }
    }
}
