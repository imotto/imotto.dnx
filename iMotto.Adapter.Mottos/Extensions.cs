using iMotto.Adapter.Mottos.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace iMotto.Adapter.Mottos
{
    public static class Extensions
    {
        public static void AddMottoAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MottosAdapter>();
            serviceCollection.AddSingleton<AddCollectionHandler>();
            serviceCollection.AddSingleton<AddCollectionMottoHandler>();
            serviceCollection.AddSingleton<AddMottoHandler>();
            serviceCollection.AddSingleton<AddReviewHandler>();
            serviceCollection.AddSingleton<AddVoteHandler>();
            serviceCollection.AddSingleton<DelCollectionMottoHandler>();

            serviceCollection.AddSingleton<DelReviewHandler>();
            serviceCollection.AddSingleton<LoveCollectionHandler>();
            serviceCollection.AddSingleton<LoveMottoHandler>();
            serviceCollection.AddSingleton<UnloveCollectionHandler>();
            serviceCollection.AddSingleton<UnloveMottoHandler>();
            serviceCollection.AddSingleton<UpdateCollectionHandler>();
            serviceCollection.AddSingleton<VoteReviewHandler>();
        }

        public static void UseMottoAdapter(this IApplicationBuilder app)
        {
            var factory = app.ApplicationServices.GetService<AdapterFactory>();
            factory.Register(Constants.ADAPTER_CODE, typeof(MottosAdapter));
        }
    }
}
