using iMotto.Adapter.Users.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace iMotto.Adapter.Users
{
    public static class Extensions
    {
        public static void AddUserAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<UsersAdapter>();
            serviceCollection.AddSingleton<AddAddressHandler>();
            serviceCollection.AddSingleton<AddBanHandler>();
            serviceCollection.AddSingleton<AddFollowHandler>();
            serviceCollection.AddSingleton<AddRelAccountHandler>();
            serviceCollection.AddSingleton<AddReportHandler>();

            serviceCollection.AddSingleton<AddUserHandler>();
            serviceCollection.AddSingleton<DelBanHandler>();
            serviceCollection.AddSingleton<ExchangeHandler>();
            serviceCollection.AddSingleton<ModifyPasswordHandler>();
            serviceCollection.AddSingleton<ModifySexHandler>();
            serviceCollection.AddSingleton<ModifyThumbHandler>();
            serviceCollection.AddSingleton<ModifyUserNameHandler>();
            serviceCollection.AddSingleton<PrepareExchangeHandler>();
            serviceCollection.AddSingleton<ReceiveAwardHandler>();
            serviceCollection.AddSingleton<ReceiveGiftHandler>();
            serviceCollection.AddSingleton<ResetPasswordHandler>();
            serviceCollection.AddSingleton<ReviewGiftHandler>();

            serviceCollection.AddSingleton<SendMsgHandler>();
            serviceCollection.AddSingleton<SetAwardAddressHandler>();
            serviceCollection.AddSingleton<SetDefaultAddressHandler>();
            serviceCollection.AddSingleton<SetNoticeReadHandler>();
            serviceCollection.AddSingleton<UserLoginHandler>();
            serviceCollection.AddSingleton<UserLogoutHandler>();

        }

        public static void UseUserAdapter(this IApplicationBuilder app)
        {
            var factory = app.ApplicationServices.GetService<AdapterFactory>();
            factory.Register(Constants.ADAPTER_CODE, typeof(UsersAdapter));
        }
    }
}
