using System;
using Boxsie.Wrapplication;
using DotNexus.App.Hubs;
using DotNexus.Core.Nexus;
using DotNexus.Identity;
using DotNexus.Jobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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


            services.AddHttpClient<INexusClient, NexusClient>();
            services.AddSingleton<NexusNodeParameters>(new NexusNodeParameters
            {
                Url = "http://serves:8080/;",
                Username = "username",
                Password = "password",
                Settings = new NexusNodeSettings
                {
                    ApiSessions = true,
                    IndexHeight = true
                }
            });
            services.AddTransient<NexusNode>();

            services.AddTransient<INodeManager, NodeManager>();
            services.AddTransient<IUserManager, UserManager>();

            services.AddTransient<INexusServiceFactory, NexusServiceFactory>();
            services.AddTransient<IJobFactory, JobFactory>();

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
}
