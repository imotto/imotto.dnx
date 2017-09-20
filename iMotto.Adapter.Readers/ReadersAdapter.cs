using iMotto.Adapter.Readers.Handlers;
using System;

namespace iMotto.Adapter.Readers
{
    public class ReadersAdapter : AdapterBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ReadersAdapter(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public override IHandler GetHandler(string code)
        {
            IHandler handler;
            switch (code)
            {
                case Constants.HANDLER_READ_STATISTICS: handler = _serviceProvider.GetHandler<ReadStatisticsHandler>(); break;
                case Constants.HANDLER_READ_MOTTOS: handler = _serviceProvider.GetHandler<ReadMottosHandler>(); break;
                case Constants.HANDLER_READ_MOTTO_REVIEWS: handler = _serviceProvider.GetHandler<ReadMottoReviewsHandler>(); break;
                case Constants.HANDLER_READ_MOTTO_VOTES: handler = _serviceProvider.GetHandler<ReadMottoVotesHandler>(); break;
                case Constants.HANDLER_READ_RECRUITS: handler = _serviceProvider.GetHandler<ReadRecruitsHandler>(); break;
                case Constants.HANDLER_READ_RECRUIT_MOTTOS: handler = _serviceProvider.GetHandler<ReadRecruitMottosHandler>(); break;
                case Constants.HANDLER_READ_COLLECTIONS: handler = _serviceProvider.GetHandler<ReadCollectionsHandler>(); break;
                case Constants.HANDLER_READ_COLLECTION_MOTTOS: handler = _serviceProvider.GetHandler<ReadCollectionMottosHandler>(); break;
                case Constants.HANDLER_READ_TAGS: handler = _serviceProvider.GetHandler<ReadTagsHandler>(); break;
                case Constants.HANDLER_READ_TAG_COLLECTIONS: handler = _serviceProvider.GetHandler<ReadTagCollectionsHandler>(); break;
                case Constants.HANDLER_READ_USER: handler = _serviceProvider.GetHandler<ReadUserHandler>(); break;
                case Constants.HANDLER_READ_USER_STATISTICS: handler = _serviceProvider.GetHandler<ReadUserStatisticsHandler>(); break;
                case Constants.HANDLER_READ_USER_MOTTOS: handler = _serviceProvider.GetHandler<ReadUserMottosHandler>(); break;
                case Constants.HANDLER_READ_USER_FOLLOWERS: handler = _serviceProvider.GetHandler<ReadUserFollowersHandler>(); break;
                case Constants.HANDLER_READ_USER_FOLLOWS: handler = _serviceProvider.GetHandler<ReadUserFollowsHandler>(); break;
                case Constants.HANDLER_READ_USER_BANS: handler = _serviceProvider.GetHandler<ReadUserBansHandler>(); break;
                case Constants.HANDLER_READ_USER_COLLECTIONS: handler = _serviceProvider.GetHandler<ReadUserCollectionsHandler>(); break;
                case Constants.HANDLER_READ_USER_RECRUITS: handler = _serviceProvider.GetHandler<ReadUserRecruitsHandler>(); break;
                case Constants.HANDLER_READ_USER_LOVED_MOTTOS: handler = _serviceProvider.GetHandler<ReadUserLovedMottosHandler>(); break;
                case Constants.HANDLER_READ_USER_LOVED_COLLECTIONS: handler = _serviceProvider.GetHandler<ReadUserLovedCollectionsHandler>(); break;
                case Constants.HANDLER_READ_MY_SCORE_RECORD: handler = _serviceProvider.GetHandler<ReadScoreRecordHandler>(); break;
                case Constants.HANDLER_READ_MY_BILL_RECORD: handler = _serviceProvider.GetHandler<ReadBillRecordHandler>(); break;
                case Constants.HANDLER_READ_RECENT_TALK: handler = _serviceProvider.GetHandler<ReadRecentTalkHandler>(); break;
                case Constants.HANDLER_READ_TALK_MSGS: handler = _serviceProvider.GetHandler<ReadTalkMsgsHandler>(); break;
                case Constants.HANDLER_READ_NOTINCE: handler = _serviceProvider.GetHandler<ReadNoticeHandler>(); break;
                case Constants.HANDLER_READ_GIFTS: handler = _serviceProvider.GetHandler<ReadGiftsHandler>(); break;
                case Constants.HANDLER_READ_MY_ADDRESS: handler = _serviceProvider.GetHandler<ReadAddressesHandler>();break;
                case Constants.HANDLER_READ_MY_REL_ACCOUNTS:handler = _serviceProvider.GetHandler<ReadRelAccountsHandler>();break;
                case Constants.HANDLER_READ_USER_EXCHANGES:handler = _serviceProvider.GetHandler<ReadUserExchangesHandler>();break;
                case Constants.HANDLER_READ_AWARDS:handler = _serviceProvider.GetHandler<ReadAwardHandler>();break;
                case Constants.HANDLER_READ_AWARDEES:handler = _serviceProvider.GetHandler<ReadAwardeeHandler>();break;
                case Constants.HANDLER_READ_MY_BADGE:handler = _serviceProvider.GetHandler<ReadUserBadgeHandler>();break;

                default: handler = base.GetHandler(code); break;
            }

            return handler;
        }
    }
}
