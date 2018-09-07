using System.Linq;
using System.Reflection;
using Core.Models;
using Core.Services;
using IdentityServer.Controllers;
using IdentityServer.Models;
using IdentityServer.Security;
using IdentityServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Migrator = IdentityServer.Services.Migrator;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("MultiTenantDb");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            
            services.AddDbContext<TenantDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped(serviceProvider =>
            {
                var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var tenantService = serviceProvider.GetRequiredService<ITenantService>();
                return new ApplicationUserValidator(httpContext, tenantService);
            });
            
            services.AddAuthentication()
                .AddOpenIdConnect("aad", "Login with Azure AD", options =>
                {
                    var adConfig = _configuration.GetSection("AzureAd");
                    options.Authority = adConfig.GetValue<string>("Authority");
                    options.TokenValidationParameters = 
                        new TokenValidationParameters { ValidateIssuer = false };
                    options.ClientId = adConfig.GetValue<string>("ClientId");
                    options.CallbackPath = adConfig.GetValue<string>("CallbackPath");
                });

            services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
                {
                    SetPasswordRequiredOptions(identityOptions);

                    identityOptions.User.RequireUniqueEmail = false;
                    identityOptions.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserValidator<ApplicationUserValidator>();

            //Remove this Validator because we don´t need to check for a unique username! The Correct validation is in the class ApplicationUserValidator        
            var userValidator =
                services.FirstOrDefault(s => s.ImplementationType == typeof(UserValidator<ApplicationUser>));
            if (userValidator != null)
            {
                services.Remove(userValidator);
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            // Add application services.
            services.AddTransient<Migrator>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<RequireTenantAttribute>();

            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            // configure identity server with postgres db, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseNpgsql(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseNpgsql(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddProfileService<CustomProfileService>()
                .AddRedirectUriValidator<CustomRedirectValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            MigrateTenantDatabase(app);
            InitializeDatabase(app);

            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<Migrator>().Migrate();
            }
        }
        
        private void MigrateTenantDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // In Production, use something more resilient
                scope.ServiceProvider.GetRequiredService<TenantDbContext>().Database.Migrate();
            }
        }

        private void SetPasswordRequiredOptions(IdentityOptions identityOptions)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                SetPasswordRequiredOptionsForDevelopment(identityOptions);
                return;
            }

            identityOptions.Password.RequiredLength = 8;
            identityOptions.Password.RequiredUniqueChars = 2;
        }

        private void SetPasswordRequiredOptionsForDevelopment(IdentityOptions identityOptions)
        {
            identityOptions.Password.RequireDigit = false;
            identityOptions.Password.RequiredLength = 2;
            identityOptions.Password.RequiredUniqueChars = 0;
            identityOptions.Password.RequireLowercase = false;
            identityOptions.Password.RequireNonAlphanumeric = false;
            identityOptions.Password.RequireUppercase = false;
        }
    }
}