using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Accounts;
using DotNexus.App.Hubs;
using DotNexus.Assets;
using DotNexus.Core;
using DotNexus.Core.Nexus;
using DotNexus.Identity;
using DotNexus.Jobs;
using DotNexus.Ledger;
using DotNexus.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.Name = ".DotNexus.App.Auth";
                });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = ".DotNexus.App.Session";
            });

            services.AddMvc();

            var sp = services.BuildServiceProvider();
            
            services.AddHttpClient<INexusClient, NexusClient>();

            services.AddTransient<AccountService>();
            services.AddTransient<LedgerService>();
            services.AddTransient<TokenService>();
            services.AddTransient<AssetService>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddSingleton<BlockNotifyJob>();
            services.AddSingleton<BlockhainHubContext>();

            services.AddDistributedMemoryCache();
#if DEBUG
            services.AddSignalR(o => { o.EnableDetailedErrors = true; });
#else
            services.AddSignalR();
#endif
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<BlockchainHub>("/blockchainhub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("block", "block/{blockId}", new { controller = "blockchain", action = "block" });
                routes.MapRoute("transaction", "transaction/{hash}", new { controller = "blockchain", action = "transaction" });
                routes.MapRoute("default", "{controller=home}/{action=index}/{id?}");
            });
            
            serviceProvider.GetService<BlockhainHubContext>();

            Task.Run(() => Start(app));
            
        }

        private static async Task Start(IApplicationBuilder app)
        {
            var cancel = new CancellationTokenSource();

            await app.ApplicationServices.GetService<BlockNotifyJob>()
                .StartAsync(3, cancel.Token)
                .ConfigureAwait(false);
        }
    }
}
