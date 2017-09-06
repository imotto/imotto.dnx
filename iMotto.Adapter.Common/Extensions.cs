using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace iMotto.Adapter.Common
{
    public static class Extensions
    {
        public static void AddCommonAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<CommonAdapter>();
            serviceCollection.AddSingleton<RegisterDeviceHandler>();
            serviceCollection.AddSingleton<SpeedTestHandler>();
            serviceCollection.AddSingleton<UpdateHandler>();
            serviceCollection.AddSingleton<VerifyCodeHandler>();
        }

        public static void UseCommonAdapter(this IApplicationBuilder app)
        {
            var factory = app.ApplicationServices.GetService<AdapterFactory>();
            factory.Register(Constants.ADAPTER_CODE, typeof(CommonAdapter));
        }
    }
}
