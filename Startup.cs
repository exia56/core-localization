using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace core_localization
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
            services.Configure<RequestLocalizationOptions>(opts =>
            {
                IList<CultureInfo> cultures = new List<CultureInfo>{
                new CultureInfo("en"),
                new CultureInfo("zh"),
                };
                opts.DefaultRequestCulture = new RequestCulture("zh");
                opts.SupportedCultures = cultures;
                opts.SupportedUICultures = cultures;
                opts.FallBackToParentCultures = true;
                opts.FallBackToParentUICultures = true;
                opts.RequestCultureProviders = new[]{
                    new RouteDataRequestCultureProvider
                    {
                        Options = opts
                    }
                };
            });
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");//指定Resources的路徑
            services.AddMvc()
                    .AddViewLocalization(); //在Razor中使用Localizer時，需要加入這行
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}",
                    defaults: new { culture = "zh" });
                routes.MapMiddlewareRoute("{culture=zh}/{*mvcRoute}", subApp =>
                {
                    subApp.UseRequestLocalization(localizationOptions.Value);

                    subApp.UseMvc(mvcRoutes =>
                    {
                        mvcRoutes.MapRoute(
                            name: "default",
                            template: "{culture=zh}/{controller=Home}/{action=Index}/{id?}");
                    });
                });
            });
        }
    }
}
