using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using iMotto.Adapter;
using iMotto.Adapter.Common;
using iMotto.Common.Settings;
using Microsoft.Extensions.DependencyInjection.Extensions;
using iMotto.Adapter.Users;
using iMotto.Adapter.Mottos;

namespace iMotto.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ConsulSetting>(Configuration.GetSection("Consul"));
            services.TryAddSingleton<ISettingProvider, SettingProvider>();

            services.AddDataDapper();
            services.AddCache();
            services.AddAdapter();
            services.AddCommonAdapter();
            services.AddUserAdapter();
            services.AddMottoAdapter();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCache();
            app.UseCommonAdapter();
            app.UseUserAdapter();
            app.UseMottoAdapter();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "adapter",
                    template: "Api/{code}",
                    defaults: new { controller = "Adapter", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
