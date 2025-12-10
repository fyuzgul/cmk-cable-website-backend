using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using CmkCable.Entities;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using System.Linq;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace CmkCable.Business.Concrete
{
    public class EmailManager
    {
        private IGetOfferRepository _getOfferRepository;
        private IContactRequestRepository _contactRequestRepository;
        private ICareerInformationRepository _careerInformationRepository;
        private IManagerMailRepository _managerMailRepository;
        private readonly IConfiguration _configuration;
        
        // Brevo HTTP API configuration - cached values
        private readonly string _brevoApiKey;
        private readonly string _brevoSenderEmail;
        private readonly string _brevoSenderName;
        
        private string BrevoApiKey => _brevoApiKey;
        private string BrevoSenderEmail => _brevoSenderEmail;
        private string BrevoSenderName => _brevoSenderName;
        private bool IsBrevoApiConfigured => !string.IsNullOrWhiteSpace(BrevoApiKey);

        private const string BrevoApiEndpoint = "https://api.brevo.com/v3/smtp/email";
        private static readonly HttpClient BrevoHttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
        private static readonly JsonSerializerOptions BrevoJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        
        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _getOfferRepository = new GetOfferRepository();
            _contactRequestRepository = new ContactRequestRepository();
            _careerInformationRepository = new CareerInformationRepository();
            _managerMailRepository = new ManagerMailRepository();
            
            // Initialize Brevo configuration with detailed logging
            Console.WriteLine("=== Initializing Brevo Configuration ===");
            
            // Try to read from environment variables first
            var envApiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY");
            var envSenderEmail = Environment.GetEnvironmentVariable("BREVO_SENDER_EMAIL");
            var envSenderName = Environment.GetEnvironmentVariable("BREVO_SENDER_NAME");
            
            Console.WriteLine($"[DEBUG] BREVO_API_KEY from env: {(string.IsNullOrEmpty(envApiKey) ? "NOT SET" : "SET (length: " + envApiKey.Length + ")")}");
            Console.WriteLine($"[DEBUG] BREVO_SENDER_EMAIL from env: {envSenderEmail ?? "NOT SET"}");
            Console.WriteLine($"[DEBUG] BREVO_SENDER_NAME from env: {envSenderName ?? "NOT SET"}");
            
            // Try to read from configuration (supports both Brevo:ApiKey and BREVO__API__KEY format)
            var configApiKey = _configuration["Brevo:ApiKey"] ?? _configuration["BREVO__API__KEY"];
            var configSenderEmail = _configuration["Brevo:SenderEmail"] ?? _configuration["BREVO__SENDER__EMAIL"];
            var configSenderName = _configuration["Brevo:SenderName"] ?? _configuration["BREVO__SENDER__NAME"];
            
            Console.WriteLine($"[DEBUG] Brevo:ApiKey from config: {(string.IsNullOrEmpty(configApiKey) ? "NOT SET" : "SET")}");
            Console.WriteLine($"[DEBUG] Brevo:SenderEmail from config: {configSenderEmail ?? "NOT SET"}");
            Console.WriteLine($"[DEBUG] Brevo:SenderName from config: {configSenderName ?? "NOT SET"}");
            
            // Set values with fallback chain
            _brevoApiKey = envApiKey ?? configApiKey;
            _brevoSenderEmail = envSenderEmail ?? 
                               Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL") ?? 
                               configSenderEmail ??
                               _configuration["Smtp:FromEmail"] ?? 
                               "runner@cmkkablo.com";
            _brevoSenderName = envSenderName ?? 
                              Environment.GetEnvironmentVariable("SMTP_FROM_NAME") ?? 
                              configSenderName ??
                              _configuration["Smtp:FromName"] ?? 
                              "CMK KABLO";
            
            Console.WriteLine($"[DEBUG] Final BrevoApiKey: {(string.IsNullOrEmpty(_brevoApiKey) ? "NOT CONFIGURED" : "CONFIGURED (length: " + _brevoApiKey.Length + ")")}");
            Console.WriteLine($"[DEBUG] Final BrevoSenderEmail: {_brevoSenderEmail}");
            Console.WriteLine($"[DEBUG] Final BrevoSenderName: {_brevoSenderName}");
            Console.WriteLine($"[DEBUG] IsBrevoApiConfigured: {IsBrevoApiConfigured}");
            Console.WriteLine("=== Brevo Configuration Initialization Complete ===");
        }

        // Parameterless constructor for use without dependency injection
        public EmailManager()
        {
            _configuration = null; // Will use hardcoded values
            _getOfferRepository = new GetOfferRepository();
            _contactRequestRepository = new ContactRequestRepository();
            _careerInformationRepository = new CareerInformationRepository();
            _managerMailRepository = new ManagerMailRepository();
            
            // Initialize Brevo configuration from environment variables only
            Console.WriteLine("=== Initializing Brevo Configuration (No DI) ===");
            
            _brevoApiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY");
            _brevoSenderEmail = Environment.GetEnvironmentVariable("BREVO_SENDER_EMAIL") ??
                               Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL") ?? 
                               "runner@cmkkablo.com";
            _brevoSenderName = Environment.GetEnvironmentVariable("BREVO_SENDER_NAME") ??
                              Environment.GetEnvironmentVariable("SMTP_FROM_NAME") ?? 
                              "CMK KABLO";
            
            Console.WriteLine($"[DEBUG] BrevoApiKey: {(string.IsNullOrEmpty(_brevoApiKey) ? "NOT CONFIGURED" : "CONFIGURED")}");
            Console.WriteLine($"[DEBUG] BrevoSenderEmail: {_brevoSenderEmail}");
            Console.WriteLine($"[DEBUG] BrevoSenderName: {_brevoSenderName}");
            Console.WriteLine("=== Brevo Configuration Initialization Complete ===");
        }

        public async Task SendOfferEmailAsync(string subject, GetOffer offerDetails)
        {
            var to_emails = _managerMailRepository.GetByType("offer");

            var htmlBody = $@"
                <style>
                    table {{
                        width: 100%;
                        border-collapse: collapse;
                        margin: 20px 0;
                    }}
                    th, td {{
                        border: 1px solid #ddd;
                        padding: 12px;
                        text-align: left;
                    }}
                    th {{
                        background-color: #f5f5f5;
                    }}
                    tr:nth-child(even) {{
                        background-color: #f9f9f9;
                    }}
                </style>
                <h1>Teklif Detayları</h1>
                <table>
                    <tr><th>Alan</th><th>Değer</th></tr>
                    <tr><td>Ad</td><td>{offerDetails.FirstName}</td></tr>
                    <tr><td>Soyad</td><td>{offerDetails.LastName}</td></tr>
                    <tr><td>Work Email</td><td>{offerDetails.WorkEmail}</td></tr>
                    <tr><td>Rol</td><td>{offerDetails.Role?.Name ?? "Belirtilmemiş"}</td></tr>
                    <tr><td>Ülke</td><td>{offerDetails.Country}</td></tr>
                    <tr><td>Şirket</td><td>{offerDetails.Company}</td></tr>
                    <tr><td>Şirket Türü</td><td>{offerDetails.CompanyType?.Name ?? "Belirtilmemiş"}</td></tr>
                    <tr><td>Telefon</td><td>{offerDetails.TelephoneNumber}</td></tr>
                    <tr><td>Yardım Türü</td><td>{offerDetails.HelpType?.Name ?? "Belirtilmemiş"}</td></tr>
                    <tr><td>Mesaj</td><td>{offerDetails.Message}</td></tr>
                    <tr><td>IP Adresi</td><td>{offerDetails.IpAddress ?? "Belirtilmemiş"}</td></tr>
                    <tr><td>Açık Rıza</td><td>{(offerDetails.AcikRiza ? "Evet" : "Hayır")}</td></tr>
                    <tr><td>Oluşturulma Tarihi</td><td>{offerDetails.CreatedAt:dd/MM/yyyy HH:mm}</td></tr>
                </table>";

            var plainTextBody = $"Teklif Detayları - {offerDetails.FirstName} {offerDetails.LastName}\nEmail: {offerDetails.WorkEmail}\nŞirket: {offerDetails.Company}";

            // Send emails to all recipients using Brevo API
            List<string> failedEmails = new List<string>();
            foreach (var emailRecord in to_emails)
            {
                if (!string.IsNullOrEmpty(emailRecord.Email))
                {
                    try
                    {
                        await SendEmailWithBrevoApiAsync(
                            emailRecord.Email, 
                            subject, 
                            htmlBody, 
                            plainTextBody
                        );
                        
                        Console.WriteLine($"Successfully sent offer email to {emailRecord.Email}");
                    }
                    catch (Exception emailEx)
                    {
                        failedEmails.Add($"{emailRecord.Email}: {emailEx.Message}");
                        Console.WriteLine($"[ERROR] Failed to send offer email to {emailRecord.Email}: {emailEx.Message}");
                    }
                }
            }

            if (failedEmails.Any())
            {
                var errorDetails = string.Join("; ", failedEmails);
                throw new Exception($"Some offer emails failed to send via Brevo API. Details: {errorDetails}");
            }

            Console.WriteLine("Offer email process completed successfully via Brevo API");
        }

        public async Task SendEmailAsync(string subject, ContactRequest message)
        {
            ContactRequest savedContactRequest = null;
            
            try
            {
                _contactRequestRepository.Add(message);
                savedContactRequest = message; // Keep reference for potential deletion
                Console.WriteLine($"Contact request saved to database");
                
                var to_mails = _managerMailRepository.GetByType("contact");
                
                var htmlBody = $@"
                    <style>
                        table {{
                            width: 100%;
                            border-collapse: collapse;
                            margin: 20px 0;
                        }}
                        th, td {{
                            border: 1px solid #ddd;
                            padding: 12px;
                            text-align: left;
                        }}
                        th {{
                            background-color: #f5f5f5;
                        }}
                        tr:nth-child(even) {{
                            background-color: #f9f9f9;
                        }}
                    </style>
                    <h1>İletişim Detayları</h1>
                    <table>
                        <tr><th>Alan</th><th>Değer</th></tr>
                        <tr><td>Ad Soyad</td><td>{message.FullName}</td></tr>
                        <tr><td>Email</td><td>{message.Email}</td></tr>
                        <tr><td>Telefon</td><td>{message.TelephoneNumber}</td></tr>
                        <tr><td>Adres</td><td>{message.Street}, {message.City} {message.Postcode}</td></tr>
                        <tr><td>Mesaj</td><td>{message.Message}</td></tr>
                        <tr><td>IP Adresi</td><td>{message.IpAddress ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Açık Rıza</td><td>{(message.Consent ? "Evet" : "Hayır")}</td></tr>
                        <tr><td>Oluşturulma Tarihi</td><td>{message.CreatedAt:dd/MM/yyyy HH:mm}</td></tr>
                    </table>";

                var plainTextBody = $"İletişim Mesajı - {message.FullName}\nEmail: {message.Email}\nTelefon: {message.TelephoneNumber}\nMesaj: {message.Message}";

                // Send emails to all recipients using Brevo API
                List<string> failedEmails = new List<string>();
                foreach (var emailRecord in to_mails)
                {
                    if (!string.IsNullOrEmpty(emailRecord.Email))
                    {
                        try
                        {
                            await SendEmailWithBrevoApiAsync(
                                emailRecord.Email, 
                                subject, 
                                htmlBody, 
                                plainTextBody
                            );
                            
                            Console.WriteLine($"Successfully sent contact email to {emailRecord.Email}");
                        }
                        catch (Exception emailEx)
                        {
                            failedEmails.Add($"{emailRecord.Email}: {emailEx.Message}");
                            Console.WriteLine($"[ERROR] Failed to send contact email to {emailRecord.Email}: {emailEx.Message}");
                        }
                    }
                }

                if (failedEmails.Any())
                {
                    var errorDetails = string.Join("; ", failedEmails);
                    throw new Exception($"Some contact emails failed to send via Brevo API. Details: {errorDetails}");
                }

                Console.WriteLine("Contact email process completed successfully via Brevo API");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Contact email error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // If we saved contact request but email failed, we should delete it
                if (savedContactRequest != null && savedContactRequest.Id > 0)
                {
                    try
                    {
                        Console.WriteLine($"Deleting contact request with ID {savedContactRequest.Id} due to email failure");
                        // Note: You'll need to implement DeleteContactRequest method in repository
                        // _contactRequestRepository.DeleteContactRequest(savedContactRequest.Id);
                        Console.WriteLine("Contact request deletion not implemented yet");
                    }
                    catch (Exception deleteEx)
                    {
                        Console.WriteLine($"Failed to delete contact request: {deleteEx.Message}");
                    }
                }
                
                throw new Exception($"Email gönderilirken hata oluştu: {ex.Message}");
            }
        }

        private async Task SendEmailWithBrevoApiAsync(string toEmail, string subject, string htmlContent, string plainTextContent = null, byte[] attachmentData = null, string attachmentName = null, string attachmentType = null)
        {
            if (!IsBrevoApiConfigured)
            {
                throw new InvalidOperationException("Brevo API key is not configured");
            }

            if (string.IsNullOrEmpty(BrevoSenderEmail))
            {
                throw new InvalidOperationException("Brevo sender email is not configured");
            }

            try
            {
                Console.WriteLine("[INFO] Sending email via Brevo HTTP API...");
                Console.WriteLine($"[INFO] Brevo sender: {BrevoSenderEmail} ({BrevoSenderName})");
                Console.WriteLine($"[INFO] Recipient: {toEmail}");
                Console.WriteLine($"[INFO] Subject: {subject}");
                Console.WriteLine($"[INFO] Has attachment: {attachmentData != null && !string.IsNullOrEmpty(attachmentName)}");

                var payload = new
                {
                    sender = new
                    {
                        email = BrevoSenderEmail,
                        name = BrevoSenderName
                    },
                    to = new[]
                    {
                        new { email = toEmail }
                    },
                    subject = subject,
                    htmlContent = htmlContent,
                    textContent = string.IsNullOrWhiteSpace(plainTextContent) ? null : plainTextContent,
                    attachment = (attachmentData != null && !string.IsNullOrEmpty(attachmentName))
                        ? new[]
                        {
                            new
                            {
                                name = attachmentName,
                                content = Convert.ToBase64String(attachmentData),
                                type = string.IsNullOrWhiteSpace(attachmentType) ? "application/octet-stream" : attachmentType
                            }
                        }
                        : null
                };

                var jsonPayload = JsonSerializer.Serialize(payload, BrevoJsonOptions);

                using (var request = new HttpRequestMessage(HttpMethod.Post, BrevoApiEndpoint))
                {
                    request.Headers.TryAddWithoutValidation("api-key", BrevoApiKey);
                    request.Headers.TryAddWithoutValidation("accept", "application/json");
                    request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await BrevoHttpClient.SendAsync(request);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[ERROR] Brevo API responded with {(int)response.StatusCode}: {responseBody}");
                        throw new InvalidOperationException($"Brevo API email sending failed with status {(int)response.StatusCode}. Response: {responseBody}");
                    }

                    Console.WriteLine($"[SUCCESS] Brevo API email sent successfully to {toEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Brevo API email sending failed: {ex.Message}");
                throw;
            }
        }

        private void LogFailedEmail(string emailType, string recipient, string error, object data = null)
        {
            try
            {
                var logMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Failed to send {emailType} email to {recipient}. Error: {error}";
                if (data != null)
                {
                    logMessage += $" Data: {System.Text.Json.JsonSerializer.Serialize(data)}";
                }
                
                Console.WriteLine(logMessage);
                
                // In production, you might want to log to a file or database
                // File.AppendAllText("failed_emails.log", logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log failed email: {ex.Message}");
            }
        }

        public async Task SendCareerEmailAsync(string toEmail, string subject, CareerInformation careerInformation, IFormFile attachmentFile)
        {
            CareerInformation savedCareerInfo = null;
            List<ManagerMail> to_mails = null;
            
            try
            {
                // Validate input parameters
                if (careerInformation == null)
                {
                    throw new ArgumentNullException(nameof(careerInformation), "Career information cannot be null");
                }

                if (string.IsNullOrEmpty(careerInformation.FullName))
                {
                    throw new ArgumentException("Full name is required", nameof(careerInformation));
                }

                if (string.IsNullOrEmpty(careerInformation.Email))
                {
                    throw new ArgumentException("Email is required", nameof(careerInformation));
                }

                Console.WriteLine($"Starting career email process for: {careerInformation.FullName} ({careerInformation.Email})");

                // First, save the career information to database
                try
                {
                    savedCareerInfo = _careerInformationRepository.CreateCareerInformation(careerInformation);
                    Console.WriteLine($"Career information saved to database with ID: {savedCareerInfo?.Id}");
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"Failed to save career information to database: {dbEx.Message}");
                    Console.WriteLine($"Stack trace: {dbEx.StackTrace}");
                    throw new Exception($"Failed to save career information: {dbEx.Message}", dbEx);
                }
                
                // Get manager emails for career type
                try
                {
                    to_mails = _managerMailRepository.GetByType("career");
                    Console.WriteLine($"Retrieved {to_mails?.Count ?? 0} manager emails for career type");
                }
                catch (Exception repoEx)
                {
                    Console.WriteLine($"Failed to retrieve manager emails: {repoEx.Message}");
                    Console.WriteLine($"Stack trace: {repoEx.StackTrace}");
                    throw new Exception($"Failed to retrieve manager emails: {repoEx.Message}", repoEx);
                }
                
                // Check if any manager emails are found
                if (to_mails == null || !to_mails.Any())
                {
                    // Fallback to the provided email if no manager emails are configured
                    if (!string.IsNullOrEmpty(toEmail))
                    {
                        to_mails = new List<ManagerMail> { new ManagerMail { Email = toEmail } };
                        Console.WriteLine($"Using fallback email: {toEmail}");
                    }
                    else
                    {
                        throw new Exception("No recipient emails configured for career applications. Please configure manager emails for 'career' type.");
                    }
                }

                // Validate that we have valid email addresses
                var validEmails = to_mails.Where(m => !string.IsNullOrEmpty(m?.Email)).ToList();
                if (!validEmails.Any())
                {
                    throw new Exception("No valid email addresses found in manager emails");
                }

                Console.WriteLine($"Will send emails to: {string.Join(", ", validEmails.Select(m => m.Email))}");

                var htmlBody = $@"
                    <style>
                        table {{
                            width: 100%;
                            border-collapse: collapse;
                            margin: 20px 0;
                        }}
                        th, td {{
                            border: 1px solid #ddd;
                            padding: 12px;
                            text-align: left;
                        }}
                        th {{
                            background-color: #f5f5f5;
                        }}
                        tr:nth-child(even) {{
                            background-color: #f9f9f9;
                        }}
                    </style>
                    <h1>Kariyer Detayları</h1>
                    <table>
                        <tr><th>Alan</th><th>Değer</th></tr>
                        <tr><td>Ad Soyad</td><td>{careerInformation.FullName ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Telefon</td><td>{careerInformation.TelephoneNumber ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Email</td><td>{careerInformation.Email ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Cinsiyet</td><td>{careerInformation.Gender ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Medeni Durum</td><td>{careerInformation.MaritalStatus ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Askerlik Durumu</td><td>{careerInformation.MilitaryStatus ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Sürücü Belgesi</td><td>{careerInformation.DriverLicense ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Seyahat Durumu</td><td>{careerInformation.TravelAvailability ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Başvurulan Departman</td><td>{careerInformation.Department ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Referans Kaynağı</td><td>{careerInformation.ReferenceSource ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Açıklama</td><td>{careerInformation.Description ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>CV</td><td>{careerInformation.Cv?.FileName ?? "Dosya yüklenmemiş"}</td></tr>
                        <tr><td>IP Adresi</td><td>{careerInformation.IpAddress ?? "Belirtilmemiş"}</td></tr>
                        <tr><td>Açık Rıza</td><td>{(careerInformation.Consent ? "Evet" : "Hayır")}</td></tr>
                        <tr><td>Oluşturulma Tarihi</td><td>{careerInformation.CreatedAt:dd/MM/yyyy HH:mm}</td></tr>
                    </table>";

                var plainTextBody = $"Kariyer Başvurusu - {careerInformation.FullName}\nEmail: {careerInformation.Email}\nTelefon: {careerInformation.TelephoneNumber}";

                // Prepare attachment data
                byte[] attachmentData = null;
                string attachmentName = null;
                string attachmentType = null;

                if (attachmentFile != null)
                {
                    try
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await attachmentFile.CopyToAsync(memoryStream);
                            attachmentData = memoryStream.ToArray();
                            attachmentName = attachmentFile.FileName;
                            attachmentType = attachmentFile.ContentType;
                            Console.WriteLine($"CV attachment prepared: {attachmentFile.FileName} ({attachmentData.Length} bytes)");
                        }
                    }
                    catch (Exception fileEx)
                    {
                        Console.WriteLine($"Failed to process attachment file: {fileEx.Message}");
                        // Continue without attachment rather than failing completely
                    }
                }

                Console.WriteLine("Attempting to send career emails via Brevo API...");
                
                // Send emails to all recipients using Brevo API
                List<string> failedEmails = new List<string>();
                foreach (var emailRecord in validEmails)
                {
                    if (!string.IsNullOrEmpty(emailRecord?.Email))
                    {
                        try
                        {
                            await SendEmailWithBrevoApiAsync(
                                emailRecord.Email, 
                                subject, 
                                htmlBody, 
                                plainTextBody,
                                attachmentData,
                                attachmentName,
                                attachmentType
                            );
                            
                            Console.WriteLine($"Successfully sent email to {emailRecord.Email}");
                        }
                        catch (Exception emailEx)
                        {
                            var errorMsg = emailEx is InvalidOperationException ? emailEx.Message : $"Exception: {emailEx.Message}";
                            failedEmails.Add($"{emailRecord.Email}: {errorMsg}");
                            Console.WriteLine($"[ERROR] Failed to send email to {emailRecord.Email}: {errorMsg}");
                            Console.WriteLine($"[DEBUG] Exception type: {emailEx.GetType().Name}");
                            if (emailEx.InnerException != null)
                            {
                                Console.WriteLine($"[DEBUG] Inner exception: {emailEx.InnerException.Message}");
                            }
                        }
                    }
                }

                if (failedEmails.Any())
                {
                    var errorDetails = string.Join("; ", failedEmails);
                    throw new Exception($"Some emails failed to send via Brevo API. Details: {errorDetails}");
                }

                Console.WriteLine("Career email process completed successfully via Brevo API");
            }
            catch (Exception ex)
            {
                // Log the error details for debugging
                Console.WriteLine($"Career email error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Log failed email attempt
                var recipients = to_mails?.Select(m => m?.Email).Where(e => !string.IsNullOrEmpty(e)).ToList() ?? new List<string>();
                if (!string.IsNullOrEmpty(toEmail) && !recipients.Contains(toEmail))
                {
                    recipients.Add(toEmail);
                }
                
                LogFailedEmail("career", string.Join(", ", recipients), ex.Message, new { 
                    CareerId = savedCareerInfo?.Id, 
                    FullName = careerInformation?.FullName,
                    Email = careerInformation?.Email 
                });
                
                // If we saved career info but email failed, we should delete it
                if (savedCareerInfo != null && savedCareerInfo.Id > 0)
                {
                    try
                    {
                        Console.WriteLine($"Deleting career information with ID {savedCareerInfo.Id} due to email failure");
                        _careerInformationRepository.DeleteCareerInformation(savedCareerInfo.Id);
                        Console.WriteLine("Career information deleted successfully");
                    }
                    catch (Exception deleteEx)
                    {
                        Console.WriteLine($"Failed to delete career information: {deleteEx.Message}");
                        LogFailedEmail("career", "SYSTEM", $"Failed to delete career info: {deleteEx.Message}");
                    }
                }
                
                throw new Exception($"Email gönderilirken hata oluştu: {ex.Message}", ex);
            }
        }
    }
}
