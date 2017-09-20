using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadCollectionMottosHandler : BaseHandler<ReadCollectionMottosRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IMottoRepo _mottoRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadCollectionMottosHandler(ICacheManager cacheManager, IMottoRepo mottoRepo, ModelBuilder modelBuilder)
        {
            _cacheManager = cacheManager;
            _mottoRepo = mottoRepo;
            _modelBuilder = modelBuilder;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadCollectionMottosRequest reqObj)
        {
            var ret = await _mottoRepo.GetMottosByCollectionAsync(reqObj.CID, reqObj.PIndex, reqObj.PSize);

            var data = _modelBuilder.BuildMottoModels(ret);

            if (data.Count > 0)
            {
                var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                if (uInfo != null)
                {
                    await _modelBuilder.DecorateUserRelatedData(
                        data, uInfo.Item1, setAllCollected: reqObj.IsMyAlbum == 1);
                }

            }
            
            return new HandleResult<List<MottoModel>>
            {
                State = HandleStates.Success,
                Data = data
            };
        }
    }
}
