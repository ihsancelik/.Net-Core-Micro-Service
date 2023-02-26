using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Miracle.Api.Database;
using Miracle.Api.Repositories;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Miracle.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private bool isDevelopment = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSignalR();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        LifetimeValidator = LifetimeValidator,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"]))
                    };

                    // The JwtBearer scheme knows how to extract the token from the Authorization header
                    // but we will need to manually extract it from the query string in the case of requests to the hub
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = OnMessageReceived,
                        OnTokenValidated = OnTokenValidated,
                    };
                });

            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddAuthorization();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            services.AddHttpClient();

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5102;
            });

            services.Configure<KestrelServerOptions>(opts =>
            {
                opts.Listen(IPAddress.Loopback, 5102, s => s.UseHttps());
                opts.Listen(IPAddress.Loopback, 5103);
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

            #region Scoped 
            services.AddScoped<DataHelper>();
            services.AddDbContext<MainContext>();
            services.AddTransient(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddTransient<IAboutService, AboutService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IContactFormService, ContactFormService>();
            services.AddTransient<IFeedBackService, FeedbackService>();
            services.AddTransient<IContactInfoService, ContactInfoService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IMarketService, MarketService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<ILiveTicketService, LiveTicketService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddTransient<ISmtpSettingService, SmtpSettingService>();
            services.AddTransient<ISliderService, SliderService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IVersionInfoService, VersionInfoService>();

            services.AddTransient<CancellationTokenManager>();
            services.AddTransient<IMessageGeneratorService, MessageGeneratorService>();
            services.AddScoped<SetupManagerService>();
            services.AddScoped<ImageManagerService>();

            services.AddTransient<CurrencyService>();
            services.AddScoped<HTTPManagerService>();
            #endregion

            CoreRegisters(services);

            services.AddLogging();
        }

        private void CoreRegisters(IServiceCollection services)
        {
            services.AddDbContext<Core.Api.Database.MainContext>();
            services.AddTransient<Core.Api.Services.IUserService, Core.Api.Services.UserService>();
            services.AddTransient<Core.Api.Services.ICompanyService, Core.Api.Services.CompanyService>();
        }
        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
        private Task OnMessageReceived(MessageReceivedContext context)
        {
            if (context.Request.Query.ContainsKey("access_token"))
            {
                context.Token = context.Request.Query["access_token"];
            }
            return Task.CompletedTask;
        }
        private Task OnTokenValidated(TokenValidatedContext context)
        {
            try
            {
                int.TryParse(context.Principal.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value, out int userId);
                var userService = context.HttpContext.RequestServices.GetRequiredService<Core.Api.Services.IUserService>();
                var user = userService.Get(userId);
                
                if (user == null)
                {
                    context.Fail("Invalid User");
                    return Task.CompletedTask;
                }

                if (user.WebToken == null)
                {
                    context.Fail("Invalid Token");
                    return Task.CompletedTask;
                }

                var requestToken = context.HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(requestToken))
                    requestToken = context.HttpContext.Request.Query["access_token"].ToString();
                else
                    requestToken = requestToken.Substring(7);

                if (user.WebToken != requestToken)
                {
                    context.Fail("Invalid Token");
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                context.Fail(ex);
            }

            return Task.CompletedTask;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine($"DEVELOPMENT MODE: {isDevelopment}");

            if (!isDevelopment)
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(s =>
            {
                s.WithOrigins("https://test.test","*")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials();
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TicketHub>("/chat", options =>
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                });
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "StaticFiles")),
                RequestPath = "/api/Files",
            });
        }
    }
}