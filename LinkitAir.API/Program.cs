using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LinkitAir.API
{
    public class Program
    {
        /*The Startup class is specified to the app when the app's host is built. 
        The app's host is built when Build is called on the host builder in the Program class. 
        The Startup class is usually specified by calling the WebHostBuilderExtensions.UseStartup<TStartup> method on the host builder
        */
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /*The host provides services that are available to the Startup class constructor. 
        The app adds additional services via ConfigureServices. 
        Both the host and app services are then available in Configure and throughout the app.*/
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
