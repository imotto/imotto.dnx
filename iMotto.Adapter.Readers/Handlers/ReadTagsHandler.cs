using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadTagsHandler:BaseHandler<ReadTagsRequest>
    {
        private readonly ICollectionRepo _collectionRepo;

        public ReadTagsHandler(ICollectionRepo collectionRepo)
        {
            this._collectionRepo = collectionRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadTagsRequest reqObj)
        {
            List<Tag> tags = await _collectionRepo.GetTagsAsync(reqObj.PIndex, reqObj.PSize);

            return new HandleResult<List<Tag>>
            {
                State = HandleStates.Success,
                Data = tags
            };
        }
    }
}
