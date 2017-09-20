using iMotto.Adapter.Mottos.Handlers;
using System;

namespace iMotto.Adapter.Mottos
{
    public class MottosAdapter : AdapterBase
    {
        private readonly IServiceProvider _serviceProvider;

        public MottosAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IHandler GetHandler(string code)
        {
            IHandler handler;

            switch (code)
            {
                case Constants.HANDLER_ADD_MOTTO: handler = _serviceProvider.GetHandler<AddMottoHandler>(); break;
                case Constants.HANDLER_ADD_REVIEW: handler = _serviceProvider.GetHandler<AddReviewHandler>(); break;
                case Constants.HANDLER_ADD_TREASURE: handler = _serviceProvider.GetHandler<AddCollectionHandler>(); break;
                case Constants.HANDLER_ADD_COLLECTION_MOTTO: handler = _serviceProvider.GetHandler<AddCollectionMottoHandler>(); break;
                case Constants.HANDLER_ADD_VOTE: handler = _serviceProvider.GetHandler<AddVoteHandler>(); break;
                case Constants.HANDLER_DEL_REVIEW: handler = _serviceProvider.GetHandler<DelReviewHandler>(); break;
                case Constants.HANDLER_DEL_TREASURE_MOTTO: handler = _serviceProvider.GetHandler<DelCollectionMottoHandler>(); break;
                case Constants.HANDLER_LOVE_MOTTO: handler = _serviceProvider.GetHandler<LoveMottoHandler>(); break;
                case Constants.HANDLER_LOVE_TREASURE: handler = _serviceProvider.GetHandler<LoveCollectionHandler>(); break;
                case Constants.HANDLER_UNLOVE_MOTTO: handler = _serviceProvider.GetHandler<UnloveMottoHandler>(); break;
                case Constants.HANDLER_UNLOVE_TREASURE: handler = _serviceProvider.GetHandler<UnloveCollectionHandler>(); break;
                case Constants.HANDLER_VOTE_REVIEW: handler = _serviceProvider.GetHandler<VoteReviewHandler>(); break;
                case Constants.HANDLER_UPDATE_COLLECTION:handler = _serviceProvider.GetHandler<UpdateCollectionHandler>();break;

                default:
                    handler = base.GetHandler(code);
                    break;
            }

            return handler;
        }
    }
}
