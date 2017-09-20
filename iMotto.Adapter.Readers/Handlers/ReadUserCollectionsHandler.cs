using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserCollectionsHandler:BaseHandler<ReadUserCollectionsRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICollectionRepo _collectionRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadUserCollectionsHandler(ICacheManager cacheManager, ICollectionRepo collectionRepo, ModelBuilder modelBuilder) 
        {
            _cacheManager = cacheManager;
            _collectionRepo = collectionRepo;
            _modelBuilder = modelBuilder;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserCollectionsRequest reqObj)
        {
            List<Collection> collections = await _collectionRepo
                .GetCollectionsByUserAsync(reqObj.UID, reqObj.PIndex, reqObj.PSize);
            
            var data = _modelBuilder.BuildAlbumModels(collections, reqObj.UID);

            if (reqObj.MID != 0)
            {
                //用户收藏微言时使用
                var cids = await _cacheManager.GetCache<ICollectionCache>().WhichHasContainsMID(reqObj.MID, data.Select(d => d.ID));

                foreach (var item in data.Where(d => cids.Contains(d.ID)))
                {
                    item.ContainsMID = reqObj.MID;
                }
            }
            else
            {
                //纯读取用户珍藏时使用
                if (data.Count > 0)
                {
                    var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                    if (uInfo != null)
                    {
                        if (reqObj.UID != uInfo.Item1)
                        {                            
                            await _modelBuilder.DecorateUserRelatedData(
                                data, uInfo.Item1);
                        }
                    }
                }
            }

            return new HandleResult<List<AlbumModel>>
            {
                State = HandleStates.Success,
                Data = data,
                Msg=""
            };
        }
    }
}
