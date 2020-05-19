using Maktoob.Application;
using Maktoob.CrossCuttingConcerns;
using Maktoob.CrossCuttingConcerns.Options;
using Maktoob.Domain;
using Maktoob.Infrastructure;
using Maktoob.Persistance;
using Maktoob.Persistance.Contexts;
using Maktoob.Persistance.Extensions.Mongo;
using Maktoob.SPA.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Maktoob.SPA
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
            services.AddDbContext<MaktoobDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SupportedCultures = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("ar"),
                };
            });

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            //services.AddIdentityCore<User>(options =>
            //{

            //    // Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = true;

            //    options.SignIn.RequireConfirmedAccount = false;
            //    options.SignIn.RequireConfirmedEmail = false;
            //    options.SignIn.RequireConfirmedPhoneNumber = false;
            //})
            //    .AddSignInManager()
            //    .AddDefaultTokenProviders()
            //    .AddErrorDescriber<GErrorDescriber>()
            //    .AddEntityFrameworkStores<MaktoobDbContext>();
            //services.AddIdentity<User, Role>(options =>
            //{

            //    // Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = true;

            //    options.SignIn.RequireConfirmedAccount = false;
            //    options.SignIn.RequireConfirmedEmail = false;
            //    options.SignIn.RequireConfirmedPhoneNumber = false;
            //}).AddEntityFrameworkStores<MaktoobDbContext>()
            //.AddErrorDescriber<GErrorDescriber>();

            services.Configure<JsonWebTokenOptions>(Configuration.GetSection("JsonWebToken"));
            services.Configure<MongoDbOptions>(Configuration.GetSection("Mongo"));

            services.AddInfrastructure();
            services.AddPersistence();
            services.AddCrossCuttingConcerns();
            services.AddDomain();
            services.AddApplication();

            services.AddControllers();
            services.AddMongoDb();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddOpenApiDocument(configure =>
            {
                configure.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                configure.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Maktoob API";
                };
            });
            //    options =>
            //{
            //    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Maktoob API", Version = "v1" });
            //    //var openApiSecurityScheme = new OpenApiSecurityScheme
            //    //{
            //    //    Description = "JWT Authorization header using the bearer scheme",
            //    //    Name = "Authorization",
            //    //    In = ParameterLocation.Header,
            //    //    Type = SecuritySchemeType.ApiKey
            //    //};
            //    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description = "JWT Authorization header using the Bearer scheme.",
            //    });
            //    //////Add Operation Specific Authorization///////
            //    options.OperationFilter<AuthOperationFilter>();
            //    //var openApiSecurityRequirement = new OpenApiSecurityRequirement();
            //    //openApiSecurityRequirement.Add(openApiSecurityScheme, new List<string> { "Bearer" });
            //    //options.AddSecurityRequirement(openApiSecurityRequirement);
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<LangMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".webmanifest"] = "application/manifest+json";

            app.UseStaticFiles(new StaticFileOptions {
                ContentTypeProvider = provider
            });
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles(new StaticFileOptions {
                    ContentTypeProvider = provider
                });
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseReDoc();
            //app.UseSwagger(options =>
            //{
            //    options.RouteTemplate = "swagger/{documentname}/swagger.json";
            //});

            //app.UseSwaggerUi3(options =>
            //{
            //    options.SwaggerEndpoint("v1/swagger.json", "Maktoob API");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                
                spa.Options.SourcePath = "ClientApp";
                
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
