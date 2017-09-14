using iMotto.Cache;
using iMotto.Cache.RedisImpl;
using iMotto.Data;
using iMotto.Data.Dapper;
using iMotto.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Api
{
    public static class Extensions
    {
        public static void AddDataDapper(this IServiceCollection services)
        {
            services.TryAddSingleton<IConnectionProvider, DapperConnectionProvider>();
            services.TryAddSingleton<IBanRepo, BanRepo>();
            services.TryAddSingleton<ICommonRepo, CommonRepo>();
            services.TryAddSingleton<IFollowRepo, FollowRepo>();
            services.TryAddSingleton<IGiftRepo, GiftRepo>();
            services.TryAddSingleton<IManageRepo, ManageRepo>();
            services.TryAddSingleton<IMottoRepo, MottoRepo>();
            services.TryAddSingleton<IMsgRepo, MsgRepo>();
            services.TryAddSingleton<IReportRepo, ReportRepo>();
            services.TryAddSingleton<IStatisticsRepo, StatisticsRepo>();
            services.TryAddSingleton<ICollectionRepo, CollectionRepo>();
            services.TryAddSingleton<IUserRepo, UserRepo>();            
        }

        public static void AddCache(this IServiceCollection services)
        {
            services.TryAddSingleton<IEventPublisher, DefaultEventPublisher>();
            services.TryAddSingleton<ICacheManager, CacheManager>();
            services.TryAddSingleton<RedisHelper>();
        }

        public static void UseCache(this IApplicationBuilder app)
        {
            var cacheManager = app.ApplicationServices.GetService<ICacheManager>();
            cacheManager.Initialize();
        }
    }
}
