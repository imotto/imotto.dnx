using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserLovedMottosHandler : BaseHandler<ReadUserLovedMottosRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IMottoRepo _mottoRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadUserLovedMottosHandler(ICacheManager cacheManager, IMottoRepo mottoRepo, ModelBuilder modelBuilder) 
        {
            _cacheManager = cacheManager;
            _mottoRepo = mottoRepo;
            _modelBuilder = modelBuilder;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ReadUserLovedMottosRequest reqObj)
        {
            var mottos = await _mottoRepo.GetUserLovedMottosAsync(reqObj.UID, reqObj.PIndex, reqObj.PSize);
            
            var data = _modelBuilder.BuildMottoModels(mottos);

            if (data.Count > 0)
            {
                var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                if (uInfo != null)
                {
                    await _modelBuilder.DecorateUserRelatedData(
                        data, uInfo.Item1, setAllLoved: reqObj.UID.Equals(uInfo.Item1));
                }
            }


            return new HandleResult<List<MottoModel>> {
                State = HandleStates.Success,
                Msg="",
                Data= data
            };

        }
    }
}
