using ProductsArchive.Application;
using ProductsArchive.Application.Common.Interfaces;
using ProductsArchive.Infrastructure;
using ProductsArchive.Infrastructure.Persistence;
using ProductsArchive.WebUI.Filters;
using ProductsArchive.WebUI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using ProductsArchive.WebUI.Middlewares;

namespace ProductsArchive.WebUI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // add application and services
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<ICurrentCultureService, CurrentCultureService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddRazorPages();

        // add supported cultures
        services.Configure<RequestLocalizationOptions>(options =>
        {
            List<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("de-DE"),
                new CultureInfo("en-GB"),
                new CultureInfo("es-ES"),
                new CultureInfo("fr-FR"),       
                new CultureInfo("it-IT")
            };
            options.DefaultRequestCulture = new RequestCulture("it-IT");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        services.Configure<ApiBehaviorOptions>(options => 
            options.SuppressModelStateInvalidFilter = true);

        services.AddSpaStaticFiles(configuration => 
            configuration.RootPath = "ClientApp/dist");

        // add swagger API docs
        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "ProductsArchive API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }

        app.UseSwaggerUi3(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
        });

        app.UseRouting();

        app.UseHttpRequestLogger();

        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();

        //
        //// https://github.com/aspnet/Localization/blob/master/src/Microsoft.AspNetCore.Localization/AcceptLanguageHeaderRequestCultureProvider.cs
        //// https://github.com/aspnet/Localization/blob/master/src/Microsoft.AspNetCore.Localization/RequestLocalizationMiddleware.cs
        var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

        if (options != null)
        {
            app.UseRequestLocalization(options.Value); // gets the Accept-Language header in the request
        }
       

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                // remove to run the front-end separately
                spa.UseAngularCliServer(npmScript: "start");

                // uncomment when running the front-end separately
                //spa.UseProxyToSpaDevelopmentServer(Configuration["SpaBaseUrl"] ?? "http://localhost:4200"); 
            }
        });
    }
}
