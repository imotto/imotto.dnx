using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
