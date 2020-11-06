using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MordyGolf.Website.Routing;
using MordyGolf.Website.Services;
using MordyGolf.Website.Services.Interfaces;

namespace MordyGolf.Website
{
    [ExcludeFromCodeCoverage]
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
            services.AddRazorPages(options => {
                options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
            })
            .AddRazorRuntimeCompilation()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("/Pages/Shared/Components/{0}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/Pages/Shared/PartialViews/{0}/{0}" + RazorViewEngine.ViewExtension);
            });
            services.AddMvc().AddRazorOptions(options =>
            {
                options.PageViewLocationFormats.Add("/Pages/Shared/Components/{0}.cshtml");
                options.PageViewLocationFormats.Add("/Pages/Shared/PartialViews/{0}.cshtml");
            });

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("en-CA"),
                    new CultureInfo("fr"),
                    new CultureInfo("fr-CA")
                };

                options.DefaultRequestCulture = new RequestCulture("en-CA");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
            });

            services.AddTransient<IComponentService, ComponentService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value.ToLower();

                // If Error page is displaying, Culture is forgotten since exception is thrown and a different thread takes over.
                // Check RawTarget for French and rewrite the path.
                if (url == "/error")
                {
                    if (GetRawUrl(context.Request).ToLower().Contains("/fr/"))
                    {
                        context.Request.Path = context.Request.Path.ToString().Insert(0, "/fr");
                    }
                    if (GetRawUrl(context.Request).ToLower().Contains("/fr-ca/"))
                    {
                        context.Request.Path = context.Request.Path.ToString().Insert(0, "/fr-CA");
                    }
                }

                await next();

                if (context.Response.StatusCode == 404)
                {
                    if (GetRawUrl(context.Request).ToLower().Contains("/fr"))
                    {
                        context.Request.Path = $"/fr/Error";
                    }
                    else
                    {
                        context.Request.Path = $"/Error";
                    }
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            var supportedCultures = new[]
            {
                new CultureInfo("en-CA"),
                new CultureInfo("fr-CA"),
                new CultureInfo("en"),
                new CultureInfo("fr")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-CA"),
                // Formatting numbers, dates, etc
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }

        /// <summary>
        /// Gets the Raw URL that would display in the address bar.
        /// Used for determining if French url was displayed when Exception occurred.
        /// </summary>
        /// <param name="request">The current HttpRequest</param>
        /// <returns>string - the RawTarget of the request</returns>
        private static string GetRawUrl(HttpRequest request)
        {
            var requestFeature = request.HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpRequestFeature>();
            return requestFeature.RawTarget;
        }
    }
}
