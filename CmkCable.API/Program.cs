using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CmkCable.API
{
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

                            // Dosya izinlerini kontrol et ve içerikleri oku
                            var certFileInfo = new FileInfo(certPath);
                            var keyFileInfo = new FileInfo(keyPath);
                            
                            System.Console.WriteLine($"Sertifika dosyası izinleri: {certFileInfo.Attributes}");
                            System.Console.WriteLine($"Anahtar dosyası izinleri: {keyFileInfo.Attributes}");

                            try
                            {
                                // Sertifika ve özel anahtarı yükle
                                var certContent = File.ReadAllText(certPath);
                                var keyContent = File.ReadAllText(keyPath);

                                // PEM formatındaki sertifika ve özel anahtarı birleştir
                                var certAndKeyContent = $"{certContent}\n{keyContent}";
                                var certBytes = System.Text.Encoding.UTF8.GetBytes(certAndKeyContent);

                                using var cert = new X509Certificate2(certBytes);
                                listenOptions.UseHttps(cert);
                            }
                            catch (System.Exception ex)
                            {
                                System.Console.WriteLine($"SSL yapılandırma hatası: {ex.Message}");
                                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                                throw;
                            }
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
