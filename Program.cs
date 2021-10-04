using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace gmc_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //  Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "D:\\GMC\\gmc-api\\gmc_api\\gmcexperterp-firebase-adminsdk.json");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
