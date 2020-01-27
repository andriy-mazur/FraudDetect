/* TODO
 * 1. Add ip restriction to the API- allow only typeform 
 * 2. Add auth to the UI
 * 3. 
 *
 *
 *
 *
 *
 *
 *
 * 
 */

namespace FraudDetect.WebApi
{
    using System.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}