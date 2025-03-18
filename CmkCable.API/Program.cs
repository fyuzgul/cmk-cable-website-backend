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
        private static readonly string CertPath = "/etc/ssl/certs/cert.crt";
        private static readonly string KeyPath = "/etc/ssl/private/cert.key";

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
                            try
                            {
                                var logger = options.ApplicationServices.GetService(typeof(ILogger<Program>)) as ILogger<Program>;

                                logger.LogInformation("Checking SSL certificate files...");
                                if (!File.Exists(CertPath))
                                {
                                    logger.LogError($"Certificate file not found at {CertPath}");
                                    throw new FileNotFoundException($"Certificate file not found at {CertPath}");
                                }

                                if (!File.Exists(KeyPath))
                                {
                                    logger.LogError($"Private key file not found at {KeyPath}");
                                    throw new FileNotFoundException($"Private key file not found at {KeyPath}");
                                }

                                logger.LogInformation("Reading certificate and private key...");
                                var certPem = File.ReadAllText(CertPath);
                                var keyPem = File.ReadAllText(KeyPath);

                                var certBytes = System.Text.Encoding.UTF8.GetBytes(certPem + keyPem);
                                var certificate = new X509Certificate2(certBytes);

                                logger.LogInformation($"Successfully loaded certificate. Subject: {certificate.Subject}");

                                listenOptions.UseHttps(new HttpsConnectionAdapterOptions
                                {
                                    ServerCertificate = certificate
                                });
                            }
                            catch (Exception ex)
                            {
                                var logger = options.ApplicationServices.GetService(typeof(ILogger<Program>)) as ILogger<Program>;
                                logger.LogError(ex, "Failed to configure HTTPS");
                                throw;
                            }
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
