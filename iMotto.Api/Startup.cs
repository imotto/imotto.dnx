using iMotto.Adapter;
using iMotto.Adapter.Common;
using iMotto.Adapter.Mottos;
using iMotto.Adapter.Readers;
using iMotto.Adapter.Users;
using iMotto.Api.Diagnostic;
using iMotto.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

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
            services.AddDataProtection()
                .DisableAutomaticKeyGeneration();

            services.AddOptions();
            services.Configure<ConsulSetting>(Configuration.GetSection("Consul"));
            services.TryAddSingleton<ISettingProvider, SettingProvider>();

            services.AddDataDapper();
            services.AddCache();
            services.AddAdapter();
            services.AddCommonAdapter();
            services.AddUserAdapter();
            services.AddMottoAdapter();
            services.AddReadAdapter();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            loggerFactory.AddNLog();
            app.AddNLogWeb();
            env.ConfigureNLog("nlog.config");


            app.UseCache();
            app.UseSignatureStore();
            app.UseCommonAdapter();
            app.UseUserAdapter();
            app.UseMottoAdapter();
            app.UseReadAdapter();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //#if DEBUG
            //将Request和Response的Body输出到日志中
            app.UseRequestResponseLogging();
            //#endif
            
            //Error handle
            app.UseExceptionHandler("/Home/Error");


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
