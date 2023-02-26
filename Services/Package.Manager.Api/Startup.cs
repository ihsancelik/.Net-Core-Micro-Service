using Library.Helpers.Mapper;
using Library.Helpers.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Package.Manager.Api.Constraints;
using Package.Manager.Api.Database;
using Package.Manager.Api.Helpers;
using Package.Manager.Api.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Package.Manager.Api
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();

            services.AddCors();

            services.AddDbContext<DataContext>(opt => { opt.UseMySql(conStrings.ConnectionString); });

            services.AddScoped<PackageService>();
            services.AddScoped<DataHelper>();
            services.AddScoped<ProductService>();

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
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"])),
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            services.Configure<KestrelServerOptions>(opts =>
            {
                opts.Listen(IPAddress.Loopback, Configuration.GetValue<int>("HttpPort"));
            });

            services.AddSwaggerGen(setup =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
            });
        }
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

            if (!Directory.Exists(PathConstraints.MPM_Libs))
                Directory.CreateDirectory(PathConstraints.MPM_Libs);
            if (!Directory.Exists(PathConstraints.MPM_Products))
                Directory.CreateDirectory(PathConstraints.MPM_Products);

            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = StaticFileOptionsConstraints.RequestPath,
                FileProvider = StaticFileOptionsConstraints.FileProvider,
                ServeUnknownFileTypes = true,
            });

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MPM API");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}