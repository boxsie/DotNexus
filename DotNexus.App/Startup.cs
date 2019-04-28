using System;
using Boxsie.Wrapplication;
using Boxsie.Wrapplication.Config;
using Boxsie.Wrapplication.Repository;
using DotNexus.App.Config;
using DotNexus.App.Domain;
using DotNexus.App.Hubs;
using DotNexus.Core.Assets.Models;
using DotNexus.Core.Nexus;
using DotNexus.Core.Tokens.Models;
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
                    options.LoginPath = "/";
                    options.LogoutPath = "/";
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

            services.AddSingleton(x =>
            {
                var config = x.GetService<CookieConfig>();

                return new CookieConstants
                {
                    GenesisIdKey = config.GenesisIdKey,
                    NodeAuthClaimResult = config.NodeAuthClaimResult,
                    NodeAuthClaimType = config.NodeAuthClaimType,
                    SessionIdKey = config.SessionIdKey,
                    UsernameKey = config.UsernameKey,
                    NodeIdClaimType = config.NodeIdClaimType
                };
            });
            
            services.AddMvc();

            //services.AddSingleton<NexusNodeEndpoint>(new NexusNodeEndpoint
            //{
            //    Url = "http://serves:8080/;",
            //    Username = "username",
            //    Password = "password",
            //    ApiSessions = true,
            //    IndexHeight = true
            //});

            // Factorys
            services.AddTransient<INexusServiceFactory, NexusServiceFactory>();
            services.AddTransient<IJobFactory, JobFactory>();

            // Connection
            services.AddHttpClient<INexusClient, NexusClient>();
            services.AddTransient<NexusNode>();
            services.AddTransient<INodeManager, NodeManager>();
            services.AddTransient<IUserManager, UserManager>();

            // Repositories
            services.AddTransient<INexusEndpointRepository, NexusEndpointRepository>();
            services.AddTransient<IRepository<NexusNodeEndpoint>, Repository<NexusNodeEndpoint>>();
            services.AddTransient<IRepository<Asset>, Repository<Asset>>();
            services.AddTransient<IRepository<Token>, Repository<Token>>();

            // Hubs
            services.AddSingleton<BlockhainHubMessenger>();

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
                routes.MapRoute("default", "{controller=connection}/{action=connect}");
            });
            
            serviceProvider.RegisterEntity<NexusNodeEndpoint>();
            serviceProvider.RegisterEntity<Asset>();
            serviceProvider.RegisterEntity<Token>();
        }
    }
}
