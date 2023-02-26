using Library.Helpers.Constraints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.HostedServices;
using Miracle.Core.Api.Middlewares;
using Miracle.Core.Api.Security;
using Miracle.Core.Api.Services;
using Miracle.Core.Api.Services.Helpers;
using Miracle.Core.Api.Services.UserWatch;
using Miracle.Core.Api.StaticDatas;
using Serilog;
using System;
using System.Net;
using System.Text;

namespace Miracle.Core.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private bool isDevelopment = true;

        private string connectionString = string.Empty;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ApiCorePathConstraints.Initialize();
            StaticDataServerInfo.Initialize();


            isDevelopment = true;
            connectionString = Configuration.GetSection("DevDb").Value;
#if RELEASE
            isDevelopment = false;
            connectionString = Configuration.GetSection("Db").Value;
#endif
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddHttpContextAccessor();

            services.AddAuthentication(s =>
            {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    LifetimeValidator = TokenValidator.LifetimeValidator,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"])),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = TokenValidator.OnTokenValidated,
                };
            });

            services.AddHostedService<ServerHostedService>();

            services.AddLogging();

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5100;
            });


            services.Configure<KestrelServerOptions>(opts =>
            {
                opts.Listen(IPAddress.Loopback, 5100, s => s.UseHttps());
                opts.Listen(IPAddress.Loopback, 5101);
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            #region Map
            services.AddDbContext<MainContext>(options => { options.UseMySql(connectionString); });
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserWatchService, UserWatchService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IVersionInfoService, VersionInfoService>();
            services.AddScoped<ISetupInfoService, SetupInfoService>();
            services.AddScoped<IProductTagService, ProductTagService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISMTPSettingService, SMTPSettingService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPriorityService, PriorityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPlatformService, PlatformService>();
            services.AddScoped<INoticeService, NoticeService>();
            services.AddScoped<IAppLibService, AppLibService>();
            services.AddScoped<IProductModuleService, ProductModuleService>();
            services.AddScoped<SetupManagerService>();
            services.AddScoped<ImageManagerService>();
            services.AddScoped<AppLibManager>();
            #endregion

            InjectOutsourceLibs(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            Console.WriteLine($"DEVELOPMENT MODE: {isDevelopment}");

            app.UseMiddleware<LoggerMiddleware>();

            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(s =>
            {
                s.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(ApiCorePathConstraints.StaticFiles),
                RequestPath = "/api/Files",
                ServeUnknownFileTypes = true,
            });
        }

        private void InjectOutsourceLibs(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var appLibService = serviceProvider.GetService<IAppLibService>();
            var applicationPartManager = serviceProvider.GetService<ApplicationPartManager>();
            DependencyManager dm = new DependencyManager(appLibService, applicationPartManager, services);
            dm.Inject();


            if (StaticDataServerInfo.DependencyExceptions.Count > 0)
            {
                foreach (var dependencyException in StaticDataServerInfo.DependencyExceptions)
                {
                    Console.WriteLine($"##### ERROR: {dependencyException.LibName} #####");
                    foreach (var exception in dependencyException.Exceptions)
                    {
                        Console.WriteLine($"{exception.Message}");
                    }
                }
            }
        }
    }
}
