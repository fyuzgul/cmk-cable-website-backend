using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using CmkCable.Entities;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using System.Linq;
using System;
using SendGrid;
using SendGrid.Helpers.Mail;
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
        
        // SendGrid Configuration - Artık configuration'dan okunuyor
        private string SendGridApiKey => 
            Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? 
            _configuration?["SendGrid:ApiKey"] ??
            "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";
        private string FromEmail => 
            Environment.GetEnvironmentVariable("SENDGRID_FROM_EMAIL") ?? 
            _configuration?["SendGrid:FromEmail"] ?? 
            "webcmkkablo@gmail.com";
        private string FromName => 
            Environment.GetEnvironmentVariable("SENDGRID_FROM_NAME") ?? 
            _configuration?["SendGrid:FromName"] ?? 
            "CMK KABLO";
        
        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _getOfferRepository = new GetOfferRepository();
            _contactRequestRepository = new ContactRequestRepository();
            _careerInformationRepository = new CareerInformationRepository();
            _managerMailRepository = new ManagerMailRepository();
        }

        // Parameterless constructor for use without dependency injection
        public EmailManager()
        {
            _configuration = null; // Will use hardcoded values
            _getOfferRepository = new GetOfferRepository();
            _contactRequestRepository = new ContactRequestRepository();
            _careerInformationRepository = new CareerInformationRepository();
            _managerMailRepository = new ManagerMailRepository();
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

            // Send emails to all recipients using SendGrid
            bool allEmailsSent = true;
            foreach (var emailRecord in to_emails)
            {
                if (!string.IsNullOrEmpty(emailRecord.Email))
                {
                    bool emailSent = await SendEmailWithSendGridAsync(
                        emailRecord.Email, 
                        subject, 
                        htmlBody, 
                        plainTextBody
                    );
                    
                    if (!emailSent)
                    {
                        allEmailsSent = false;
                        Console.WriteLine($"Failed to send offer email to {emailRecord.Email}");
                    }
                }
            }

            if (!allEmailsSent)
            {
                throw new Exception("Some offer emails failed to send via SendGrid. Please check your SendGrid configuration.");
            }

            Console.WriteLine("Offer email process completed successfully via SendGrid");
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

                // Send emails to all recipients using SendGrid
                bool allEmailsSent = true;
                foreach (var emailRecord in to_mails)
                {
                    if (!string.IsNullOrEmpty(emailRecord.Email))
                    {
                        bool emailSent = await SendEmailWithSendGridAsync(
                            emailRecord.Email, 
                            subject, 
                            htmlBody, 
                            plainTextBody
                        );
                        
                        if (!emailSent)
                        {
                            allEmailsSent = false;
                            Console.WriteLine($"Failed to send contact email to {emailRecord.Email}");
                        }
                    }
                }

                if (!allEmailsSent)
                {
                    throw new Exception("Some contact emails failed to send via SendGrid. Please check your SendGrid configuration.");
                }

                Console.WriteLine("Contact email process completed successfully via SendGrid");
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

        private async Task<bool> SendEmailWithSendGridAsync(string toEmail, string subject, string htmlContent, string plainTextContent = null, byte[] attachmentData = null, string attachmentName = null, string attachmentType = null)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(toEmail))
                {
                    Console.WriteLine("SendEmailWithSendGridAsync: toEmail is null or empty");
                    return false;
                }

                if (string.IsNullOrEmpty(subject))
                {
                    Console.WriteLine("SendEmailWithSendGridAsync: subject is null or empty");
                    return false;
                }

                if (string.IsNullOrEmpty(htmlContent))
                {
                    Console.WriteLine("SendEmailWithSendGridAsync: htmlContent is null or empty");
                    return false;
                }

                if (string.IsNullOrEmpty(SendGridApiKey))
                {
                    Console.WriteLine("SendGrid API Key not configured. Please set SendGrid:ApiKey in appsettings.json.");
                    return false;
                }

                if (string.IsNullOrEmpty(FromEmail))
                {
                    Console.WriteLine("FROM_EMAIL is not configured");
                    return false;
                }
                
                Console.WriteLine($"Preparing to send email to: {toEmail}");
                Console.WriteLine($"From: {FromEmail} ({FromName})");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"Has attachment: {attachmentData != null && !string.IsNullOrEmpty(attachmentName)}");
                
                var client = new SendGridClient(SendGridApiKey);
                var from = new EmailAddress(FromEmail, FromName);
                var to = new EmailAddress(toEmail);
                
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                
                // Add attachment if provided
                if (attachmentData != null && !string.IsNullOrEmpty(attachmentName))
                {
                    try
                    {
                        msg.AddAttachment(attachmentName, Convert.ToBase64String(attachmentData), attachmentType ?? "application/octet-stream");
                        Console.WriteLine($"Attachment added: {attachmentName} ({attachmentData.Length} bytes)");
                    }
                    catch (Exception attachEx)
                    {
                        Console.WriteLine($"Failed to add attachment: {attachEx.Message}");
                        // Continue without attachment rather than failing completely
                    }
                }
                
                Console.WriteLine("Sending email via SendGrid...");
                var response = await client.SendEmailAsync(msg);
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"SendGrid email sent successfully to {toEmail}");
                    return true;
                }
                else
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    Console.WriteLine($"SendGrid email failed: {response.StatusCode} - {responseBody}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendGrid email error for {toEmail}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
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

                Console.WriteLine("Attempting to send email via SendGrid...");
                
                // Send emails to all recipients using SendGrid
                bool allEmailsSent = true;
                foreach (var emailRecord in validEmails)
                {
                    if (!string.IsNullOrEmpty(emailRecord?.Email))
                    {
                        try
                        {
                            bool emailSent = await SendEmailWithSendGridAsync(
                                emailRecord.Email, 
                                subject, 
                                htmlBody, 
                                plainTextBody,
                                attachmentData,
                                attachmentName,
                                attachmentType
                            );
                            
                            if (!emailSent)
                            {
                                allEmailsSent = false;
                                Console.WriteLine($"Failed to send email to {emailRecord.Email}");
                            }
                            else
                            {
                                Console.WriteLine($"Successfully sent email to {emailRecord.Email}");
                            }
                        }
                        catch (Exception emailEx)
                        {
                            allEmailsSent = false;
                            Console.WriteLine($"Exception while sending email to {emailRecord.Email}: {emailEx.Message}");
                        }
                    }
                }

                if (!allEmailsSent)
                {
                    throw new Exception("Some emails failed to send via SendGrid. Please check your SendGrid configuration.");
                }

                Console.WriteLine("Career email process completed successfully via SendGrid");
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
