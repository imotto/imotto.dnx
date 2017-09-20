using System.Collections.Generic;
using System.Threading.Tasks;
using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Cache;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadCollectionsHandler : BaseHandler<ReadCollectionsRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICollectionRepo _collectionRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadCollectionsHandler(ICacheManager cacheManager,
            ICollectionRepo collectionRepo,
            ModelBuilder modelBuilder) 
        {
            _cacheManager = cacheManager;
            _collectionRepo = collectionRepo;
            _modelBuilder = modelBuilder;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadCollectionsRequest reqObj)
        {
            List<Collection> ret = await _collectionRepo.GetCollectionsAsync(reqObj.PIndex, reqObj.PSize);

            var data = _modelBuilder.BuildAlbumModels(ret);

            if (data.Count > 0)
            {
                var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                if (uInfo != null)
                {
                    await _modelBuilder.DecorateUserRelatedData(data, uInfo.Item1);
                }
            }

            return new HandleResult<List<AlbumModel>>
            {
                State = HandleStates.Success,
                Data = data,
                Msg=string.Empty
            };
        }
    }
}
