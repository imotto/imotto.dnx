using iMotto.Adapter.Readers.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace iMotto.Adapter.Readers
{

    public static class Extensions
    {
        public static void AddReadAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ModelBuilder>();
            serviceCollection.AddSingleton<ReadersAdapter>();
            serviceCollection.AddSingleton<ReadAddressesHandler>();
            serviceCollection.AddSingleton<ReadAwardeeHandler>();
            serviceCollection.AddSingleton<ReadAwardHandler>();
            serviceCollection.AddSingleton<ReadBillRecordHandler>();
            serviceCollection.AddSingleton<ReadCollectionMottosHandler>();
            serviceCollection.AddSingleton<ReadCollectionsHandler>();

            serviceCollection.AddSingleton<ReadGiftExchangesHandler>();
            serviceCollection.AddSingleton<ReadGiftsHandler>();
            serviceCollection.AddSingleton<ReadMottoReviewsHandler>();
            serviceCollection.AddSingleton<ReadMottosHandler>();
            serviceCollection.AddSingleton<ReadMottoVotesHandler>();
            serviceCollection.AddSingleton<ReadNoticeHandler>();
            serviceCollection.AddSingleton<ReadRecentTalkHandler>();

            serviceCollection.AddSingleton<ReadRecruitMottosHandler>();
            serviceCollection.AddSingleton<ReadRecruitsHandler>();
            serviceCollection.AddSingleton<ReadRelAccountsHandler>();
            serviceCollection.AddSingleton<ReadScoreRecordHandler>();
            serviceCollection.AddSingleton<ReadStatisticsHandler>();
            serviceCollection.AddSingleton<ReadTagCollectionsHandler>();
            serviceCollection.AddSingleton<ReadTagsHandler>();
            serviceCollection.AddSingleton<ReadTalkMsgsHandler>();
            serviceCollection.AddSingleton<ReadUserBadgeHandler>();
            serviceCollection.AddSingleton<ReadUserBansHandler>();

            serviceCollection.AddSingleton<ReadUserCollectionsHandler>();
            serviceCollection.AddSingleton<ReadUserExchangesHandler>();
            serviceCollection.AddSingleton<ReadUserFollowersHandler>();
            serviceCollection.AddSingleton<ReadUserFollowsHandler>();
            serviceCollection.AddSingleton<ReadUserHandler>();
            serviceCollection.AddSingleton<ReadUserLovedCollectionsHandler>();
            serviceCollection.AddSingleton<ReadUserLovedMottosHandler>();
            serviceCollection.AddSingleton<ReadUserMottosHandler>();
            serviceCollection.AddSingleton<ReadUserRecruitsHandler>();
            serviceCollection.AddSingleton<ReadUserStatisticsHandler>();

        }

        public static void UseReadAdapter(this IApplicationBuilder app)
        {
            var factory = app.ApplicationServices.GetService<AdapterFactory>();
            factory.Register(Constants.ADAPTER_CODE, typeof(ReadersAdapter));
        }
    }


    static class TheDayExtensions
    {
        /// <summary>
        /// 将DateTime转换为yyyyMMdd形式的int数值
        /// </summary>
        /// <param name="theDay"></param>
        /// <returns></returns>
        public static int ToInteger(this DateTime theDay)
        {
            return theDay.Year * 10000 + theDay.Month * 100 + theDay.Day;
        }

        public static DateTime ToDateTime(this int theDay)
        {
            if (theDay < 29990000 && theDay > 20160401)
            {
                var year = theDay / 10000;
                var month = theDay % 10000 / 100;
                var day = theDay % 100;
                try
                {
                    return new DateTime(year, month, day);
                }
                catch
                {
                }
            }

            return DateTime.Today;
        }
    }
}
