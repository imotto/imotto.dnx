using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadTagCollectionsHandler:BaseHandler<ReadTagCollectionsRequest>
    {
        private readonly ICollectionRepo _collectionRepo;

        public ReadTagCollectionsHandler(ICollectionRepo collectionRepo) 
        {
            _collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadTagCollectionsRequest reqObj)
        {
            var collections = await _collectionRepo.GetCollectionsByTagAsync(reqObj.Tag, reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<Collection>>
            {
                State=HandleStates.Success,
                Data=collections
            };
        }
    }
}
