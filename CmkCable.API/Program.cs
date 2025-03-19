using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace CmkCable.API
{
    public class Program
    {
        private static readonly string CertPath = "/etc/letsencrypt/live/cmkkablo.com/fullchain.pem";
        private static readonly string KeyPath = "/etc/letsencrypt/live/cmkkablo.com/privkey.pem";

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
                                
                                // Read certificate and private key
                                var certPem = File.ReadAllText(CertPath);
                                var keyPem = File.ReadAllText(KeyPath);

                                // Create certificate with private key
                                var certificate = X509Certificate2.CreateFromPem(
                                    certPem,
                                    keyPem
                                );

                                // Create a copy with exportable private key
                                certificate = new X509Certificate2(certificate.Export(X509ContentType.Pfx), "", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

                                logger.LogInformation($"Successfully loaded certificate. Subject: {certificate.Subject}");
                                logger.LogInformation($"Certificate has private key: {certificate.HasPrivateKey}");

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
