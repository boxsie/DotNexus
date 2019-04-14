using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNexus.Accounts;
using DotNexus.Assets;
using DotNexus.Core;
using DotNexus.Identity;
using DotNexus.Ledger;
using DotNexus.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace DotNexus.App
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/logout";
                });

            services.AddMvc();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = ".DotNexus.App.Session";
            });

            var sp = services.BuildServiceProvider();

            var serviceSettings = new NexusServiceSettings
            {
                ApiSessions = true,
                IndexHeight = true
            };

            var cs = _configuration.GetConnectionString("Node");

            services.AddTransient<AccountService>(x => 
                new AccountService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings));
            services.AddTransient<LedgerService>(x =>
                new LedgerService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings));
            services.AddTransient<TokenService>(x =>
                new TokenService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings));
            services.AddTransient<AssetService>(x =>
                new AssetService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings));

            services.AddTransient<IUserManager, UserManager>();
            services.AddDistributedMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("block", "block/{blockId}", new { controller = "blockchain", action = "block" });
                routes.MapRoute("default", "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
