using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core;
using Core.Models;
using Core.Services;
using GamesService.Models;
using GamesService.Security;
using GamesService.Services;
using Grace.AspNetCore.MVC;
using Grace.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GamesService
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TenantDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MultiTenantDb")));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddScoped((serviceProvider) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                if (httpContextAccessor.HttpContext == null)
                    return null;

                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userTenant = httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.TenantId)?.Value;
                var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("GamesDbTemplate")
                    .Replace("{tenant}", userTenant);

                var tenant = new GamesTenant
                {
                    Name = userTenant,
                    ConnectionString = connectionString,
                    Subject = userId
                };
                return tenant;
            });

            services.AddScoped((serviceProvider) =>
            {
                var tenant = serviceProvider.GetRequiredService<GamesTenant>();
                var builder = new DbContextOptionsBuilder<GamesDbContext>();
                builder.UseNpgsql(tenant.ConnectionString);
                return builder.Options;
            });

            //If Database not exists because a new tenant was registered, create this new one
            services.AddScoped(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<DbContextOptions<GamesDbContext>>();
                var context = new GamesDbContext(options);
                var exists = ((RelationalDatabaseCreator) context.GetService<IDatabaseCreator>()).Exists();
                if(!exists)
                    context.Database.Migrate();
                return context;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var authoriyUrl = Configuration.GetSection("IdentityServer").GetValue<string>("Url");
                    options.Authority = authoriyUrl;
                    options.Audience = "games-api";
                    options.RequireHttpsMetadata = false; // do not do this in production!
                });

            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MigrateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseAuthentication();
            app.UseTenantAuthentication();
            app.UseMvc();
        }

        private void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // In Production, use something more resilient
                scope.ServiceProvider.GetRequiredService<TenantDbContext>().Database.Migrate();

                //Try to move this section
                var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();
                var names = tenantService.GetAllTenantNames();
                foreach (var name in names)
                {
                    var connectionString = Configuration.GetSection("ConnectionStrings")
                        .GetValue<string>("GamesDbTemplate")
                        .Replace("{tenant}", name);
                    var builder = new DbContextOptionsBuilder<GamesDbContext>();
                    builder.UseNpgsql(connectionString);
                    var context = new GamesDbContext(builder.Options);
                    context.Database.Migrate();
                }
            }
        }
    }
}