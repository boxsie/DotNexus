using System;
using System.Threading;
using System.Threading.Tasks;
using Boxsie.Wrapplication;
using Boxsie.Wrapplication.Config;
using Boxsie.Wrapplication.Logging;
using DotNexus.App.Hubs;
using DotNexus.Core.Accounts;
using DotNexus.Core.Assets;
using DotNexus.Core.Ledger;
using DotNexus.Core.Nexus;
using DotNexus.Core.Tokens;
using DotNexus.Identity;
using DotNexus.Jobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNexus.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Bx.ConfigureServices<DotNexusApp>(services);

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
            
            services.AddHttpClient<INexusClient, NexusClient>(x => { NexusClient.ConfigureHttpClient(x, "http://serves:8080/;username;password;"); });
            services.AddSingleton<NexusSettings>(x => new NexusSettings {ApiSessions = true, IndexHeight = true});

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
            Bx.Configure(serviceProvider);

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
                routes.MapRoute("genesis", "genesis/{hash}", new { controller = "blockchain", action = "genesis" });
                routes.MapRoute("default", "{controller=home}/{action=index}/{id?}");
            });
            
            serviceProvider.GetService<BlockhainHubContext>();
        }
    }

    public class DotNexusApp : IBxApp
    {
        private readonly BlockNotifyJob _blockNotifyJob;
        private readonly CancellationTokenSource _cancellationToken;

        public DotNexusApp(BlockNotifyJob blockNotifyJob)
        {
            _blockNotifyJob = blockNotifyJob;
            _cancellationToken = new CancellationTokenSource();
        }

        public async Task StartAsync()
        {

        }
    }
}
