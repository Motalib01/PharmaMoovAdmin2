using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PharmaMoov.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // this line adds hot reload for pages. without it you need to stop/start the application before changes in front-end takes effect
            services.AddRazorPages().AddRazorRuntimeCompilation();
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-NZ");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-NZ");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-NZ") };
                options.RequestCultureProviders.Clear();
            });

            services.AddScoped<IMainHttpClient, MainHttpClient>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region "Config Init"
            //build the master configurat
            var mConfig = new ConfigMaster
            {
                WebApiBaseUrl = Configuration["WebAPI:Link"]
            };
            services.AddSingleton(mConfig);
            #endregion "Config Init"

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            #region "Session init"
            //for session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AspNetCore.Session.PharmaMoov.Admin";
                //set the browser session time to half a year
                options.IdleTimeout = TimeSpan.FromDays(150);
                options.Cookie.IsEssential = true;
            });
            #endregion "Session init"

            services.AddMvc().AddNewtonsoftJson();
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] { new CultureInfo("es-NZ") };
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-NZ");
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            #region "Session init"
            //for session
            app.UseSession();
            #endregion "Session init"

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Shop}/{action=Index}/{id?}");
            });
        }
    }
}
