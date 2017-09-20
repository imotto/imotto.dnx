using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserLovedCollectionsHandler : BaseHandler<ReadUserLovedCollectionsRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICollectionRepo _collectionRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadUserLovedCollectionsHandler(ICacheManager cacheManager, ICollectionRepo collectionRepo, ModelBuilder modelBuilder)
        {
            _cacheManager = cacheManager;
            _collectionRepo = collectionRepo;
            _modelBuilder = modelBuilder;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadUserLovedCollectionsRequest reqObj)
        {
            var collections = await _collectionRepo
                .GetUserLovedCollectionsAsync(reqObj.UID, reqObj.PIndex, reqObj.PSize);

            var data = _modelBuilder.BuildAlbumModels(collections);

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
                Msg = string.Empty,
                Data = data
            };
        }
    }
}
