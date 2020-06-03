using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Product
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                 {
                     var config = new ConfigurationBuilder().AddCommandLine(args).Build();
                     var ip = config["ip"];
                     var port = config["port"];
                     webBuilder.UseConfiguration(config);
                     webBuilder.UseUrls($"http://{ip}:{port}");
                     webBuilder.UseStartup<Startup>();
                 });
    }
}