
//using Autofac;
//using Kader.Data.DataAccessLayer;
//using Kader.Extensions;
//using Kader.Infrastructure.Jwt;
//using Kader.Infrastructure.Jwt.authentication;
//using Kader.Middlewares;
//using Kader.Middlewares.Cookies;
//using Kader.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;
//using Kader.Services.Implementations.User;
//using Spire.Xls.Core.Spreadsheet;
//using Kader.Data.DataAccessLayer.Entities;
//using Kader.Data.DataAccessLayer.Repositories.Interfaces;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Kader.Services.Implementations.Catogry;
//using System.Net.Http;
//using Kader.Services.Implementations.ShippingCompany;
//namespace Kader
//{
//    public class Startup
//    {
//        private IWebHostEnvironment _env { get; set; }
//        private SymmetricSecurityKey signingKey { get; set; }
//        public Startup(IWebHostEnvironment env)
//        {
//            _env = env; 
//            var builder = new ConfigurationBuilder()
//       .SetBasePath(env.ContentRootPath)
//       .AddJsonFile("appsettings1.json", optional: false, reloadOnChange: true)
//       .AddJsonFile($"appsettings1.{env.EnvironmentName}.json", optional: true)
//       .AddEnvironmentVariables();
//            this.Configuration = builder.Build();

//                  }
       

//        public IConfigurationRoot Configuration { get; private set; }
//        public ILifetimeScope AutofacContainer { get; private set; }
         
//        public void ConfigureServices(IServiceCollection services)
//        {
//            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Keys").GetValue<string>("signingKey")));
//            services.AddDbContext<DBContext>(options =>
//                options.UseSqlServer(Configuration.GetConnectionString("seSelocs")));
//            services.AddDbContext<ApplicationDbContext>(options =>
//               options.UseSqlServer(Configuration.GetConnectionString("seSelocs")));
//            services.AddHttpContextAccessor();
//            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
//            {
//                options.Password.RequireNonAlphanumeric = false; // Change to true if you want to require a non-alphanumeric character
//                options.Password.RequireLowercase = false; // Change to true if you want to require a lowercase letter
//                options.Password.RequireUppercase = false; // Change to true if you want to require an uppercase letter

//            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//            services.ConfigureApplicationCookie(options =>
//            {
//               options.ExpireTimeSpan = TimeSpan.FromMinutes(220);
//            }); 
//            var serviceProvider = services.BuildServiceProvider();
//            var logger = serviceProvider.GetService<ILogger<ApplicationLogs>>();
//            services.AddSingleton(typeof(ILogger), logger);
//            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(880); });
//            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, CustomClaimsPrincipalFactory<IdentityUser>>();
//            services.AddScoped<UserManager<ApplicationUser>>();
//            services.AddScoped<RoleManager<ApplicationRole>>();

//            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
//            services.Configure<JwtIssuerOptions>(options =>
//            {
//                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
//                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
//                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
//            });

//            var refreshToken = Configuration["OtoApi:RefreshToken"];
//            if (string.IsNullOrEmpty(refreshToken))
//            {
//                throw new Exception("OtoApi:RefreshToken is not configured or is missing in appsettings1.json.");
//            }

//            services.Configure<AppSettings1>(Configuration.GetSection("Keys"));
//            var tokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
//                ValidateAudience = true,
//                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = signingKey,
//                RequireExpirationTime = true,
//                ValidateLifetime = true,
//                ClockSkew = TimeSpan.Zero
//            };
//            services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//            }).AddJwtBearer(configureOptions =>
//            {
//                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
//                configureOptions.TokenValidationParameters = tokenValidationParameters;
//                configureOptions.RequireHttpsMetadata = false;
//                configureOptions.SaveToken = true;
//                configureOptions.Events = new JwtBearerEvents
//                {
//                    OnAuthenticationFailed = context =>
//                    {
//                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
//                        {
//                            context.Response.Headers.Add("Token-Expired", "true");
//                        }
//                        return Task.CompletedTask;
//                    },
//                };
//            });
//            services.AddAntiforgery();
//            services.AddSingleton<RoleCheckerMiddleware>();
//            services.AddControllersWithViews()
//                .AddMvcLocalization()
//                .AddDataAnnotationsLocalization()
//                .AddRazorPagesOptions(opt =>
//                {
//                    if (!_env.IsDevelopment())
//                    {
//                    }
//                });

//        }

//        public void ConfigureContainer(ContainerBuilder builder)
//        {
//            builder.RegisterInstance(Configuration).As<IConfiguration>();
//            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
//            builder.RegisterModule(new JwtModule());
//            builder.RegisterModule(new ServiceModule());
//            builder.Register(c => new HttpClient()).As<HttpClient>();
//            builder.RegisterType<Cookies>().As<ICookies>();
//            builder.RegisterType<OtoApiService>().AsSelf();
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }
//            else
//            {
//                //  app.UseExceptionHandler("~/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();
//            app.UseRouting();
//            app.UseSession();
//            app.UseAuthentication();
//            app.UseAuthorization();


//            //var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
//            //app.UseRequestLocalization(locOptions.Value);
//            app.UseMiddleware<JwtMiddleware>();
//            app.UseMiddleware<RoleCheckerMiddleware>();
//            app.UseEndpoints(endpoints =>
//           {
//               endpoints.MapControllerRoute(
//                    name: "Backend",
//       pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

//               endpoints.MapControllerRoute(
//                   name: "default",
//                   pattern: "{controller=Home}/{action=Index}/{id?}");
//           });
//        }
//    }
//}
