using Auth.Api.Database;
using Auth.Api.Helpers;
using Auth.Api.Middlewares;
using Auth.Api.Services;
using Auth.Api.Token;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Auth.Api
{
    public class Startup
    {
        private SQLConnectionStrings conStrings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            conStrings = new SQLConnectionStrings(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();

            services.AddDbContext<DataContext>(opt => { opt.UseMySql(conStrings.ConnectionString); });
            services.AddDbContext<CoreDataContext>(opt => { opt.UseMySql(conStrings.CoreConnectionString); });

            services.AddScoped<AuthService>();
            services.AddScoped<SettingsService>();
            services.AddScoped<TokenManager>();
            services.AddScoped<TokenTypeService>();



            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Index";
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        LifetimeValidator = TokenValidator.LifetimeValidator,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"])),
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = TokenValidator.OnTokenValidated,
                    };
                });

            services.Configure<KestrelServerOptions>(opts =>
            {
                opts.ListenLocalhost(Configuration.GetValue<int>("HttpPort"));
            });
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
                // In development
                try
                {
                    using (var scope = app.ApplicationServices.CreateScope())
                    using (var context = scope.ServiceProvider.GetService<DataContext>())
                        context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }

            app.UseMiddleware<LoggerMiddleware>();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
