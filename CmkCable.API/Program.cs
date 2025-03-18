using CmkCable.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Https;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Debug);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("https://*:1000");
                webBuilder.ConfigureKestrel(options =>
                {
                    // Sertifikayý ekleyin
                    options.ListenAnyIP(1000, listenOptions =>
                    {
                        listenOptions.UseHttps("/etc/ssl/certs/server.crt", "/etc/ssl/private/server.key");
                    });
                });
                webBuilder.UseStartup<Startup>();
            });
}
