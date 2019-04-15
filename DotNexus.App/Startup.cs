using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Accounts;
using DotNexus.Assets;
using DotNexus.Core;
using DotNexus.Core.Nexus;
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
using Microsoft.Extensions.Logging;
using NLog;

namespace DotNexus.App
{
    public class Startup
    {
        private readonly ILoggerFactory _logFactory;
        private readonly IConfiguration _configuration;

        public Startup(ILoggerFactory logFactory, IConfiguration configuration)
        {
            _logFactory = logFactory;

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

            var serviceSettings = new NexusSettings
            {
                ApiSessions = true,
                IndexHeight = true
            };

            services.AddHttpClient<INexusClient, NexusClient>();

            services.AddTransient<AccountService>(x => new AccountService(_logFactory.CreateLogger<AccountService>(), x.GetService<INexusClient>(), serviceSettings));
            services.AddTransient<LedgerService>(x => new LedgerService(_logFactory.CreateLogger<LedgerService>(), x.GetService<INexusClient>(), serviceSettings));
            services.AddTransient<TokenService>(x => new TokenService(_logFactory.CreateLogger<TokenService>(), x.GetService<INexusClient>(), serviceSettings));
            services.AddTransient<AssetService>(x => new AssetService(_logFactory.CreateLogger<AssetService>(), x.GetService<INexusClient>(), serviceSettings));

            services.AddTransient<IUserManager, UserManager>();

            services.AddTransient<BlockNotifyJob>();

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

            var t = new CancellationTokenSource();
            var n = app.ApplicationServices.GetService<BlockNotifyJob>();

            n.OnNotify = block => Task.CompletedTask;
            n.StartAsync(3, t.Token);
        }
    }
}
