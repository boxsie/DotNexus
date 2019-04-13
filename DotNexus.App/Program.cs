using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNexus.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 5000); // http:localhost:5000
                    options.Listen(IPAddress.Any, 80); // http:*:80
                    options.Listen(IPAddress.Loopback, 443,
                        listenOptions => { listenOptions.UseHttps("certificate.pfx", "password"); });
                })
                .UseStartup<Startup>();
    }
}
