using iMotto.Cache;
using iMotto.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace iMotto.Adapter
{
    public static class Extensions
    {
        public static IHandler GetHandler<T>(this IServiceProvider serviceProvider) where T : IHandler
        {
            return (serviceProvider.GetService(typeof(T)) as IHandler) ?? new DefaultHandler();
        }

        public static void AddAdapter(this IServiceCollection services)
        {
            services.AddSingleton<AdapterFactory>();
            services.TryAddSingleton<ISmsService, DayuSmsService>();
        }

        public static void UseSignatureStore(this IApplicationBuilder app)
        {
            SignatureStore.CacheManager = app.ApplicationServices.GetService<ICacheManager>();
            SignatureStore.Logger = app.ApplicationServices.GetService<ILoggerFactory>()
                                        .CreateLogger("SignatureStore");
        }
    }
}
