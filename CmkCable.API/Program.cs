using CmkCable.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;
using System.IO;

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
                    options.ListenAnyIP(1000, listenOptions =>
                    {
                        var certPath = "/etc/ssl/certs/cert.crt";
                        var keyPath = "/etc/ssl/private/cert.key";

                        // Dosyaların varlığını kontrol et
                        if (!File.Exists(certPath))
                            throw new FileNotFoundException($"SSL sertifika dosyası bulunamadı: {certPath}");
                        if (!File.Exists(keyPath))
                            throw new FileNotFoundException($"SSL anahtar dosyası bulunamadı: {keyPath}");

                        // Dosya izinlerini kontrol et
                        var certFileInfo = new FileInfo(certPath);
                        var keyFileInfo = new FileInfo(keyPath);
                        
                        Console.WriteLine($"Sertifika dosyası izinleri: {certFileInfo.Attributes}");
                        Console.WriteLine($"Anahtar dosyası izinleri: {keyFileInfo.Attributes}");

                        try
                        {
                            listenOptions.UseHttps(certPath, keyPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"SSL yapılandırma hatası: {ex.Message}");
                            Console.WriteLine($"Stack trace: {ex.StackTrace}");
                            throw;
                        }
                    });
                });
                webBuilder.UseStartup<Startup>();
            });
}
