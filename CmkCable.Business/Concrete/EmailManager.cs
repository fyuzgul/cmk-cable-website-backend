using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using CmkCable.Entities;
using System.Net;
using System.Text;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using System.Linq;
using System;

namespace CmkCable.Business.Concrete
{
    public class EmailManager
    {
        private IGetOfferRepository _getOfferRepository;
        private IContactRequestRepository _contactRequestRepository;
        private ICareerInformationRepository _careerInformationRepository;
        private IManagerMailRepository _managerMailRepository;
        public EmailManager()
        {
            _getOfferRepository = new GetOfferRepository();
            _contactRequestRepository = new ContactRequestRepository();
            _careerInformationRepository = new CareerInformationRepository();
            _managerMailRepository = new ManagerMailRepository();
        }

        public async Task SendOfferEmailAsync(string subject, GetOffer offerDetails)
        {
            var to_emails = _managerMailRepository.GetByType("offer");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CMK KABLO", "webcmkkablo@gmail.com"));

            foreach (var emailRecord in to_emails)
            {
                emailMessage.To.Add(new MailboxAddress("Recipient", emailRecord.Email));
            }

            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
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
                    </table>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            // Use the helper method for reliable email sending
            bool emailSent = await SendEmailWithFallbackAsync(emailMessage);

            if (!emailSent)
            {
                throw new Exception("All SMTP configurations failed. Please check your email configuration.");
            }
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
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("CMK KABLO", "webcmkkablo@gmail.com"));

                foreach (var emailRecord in to_mails)
                {
                    emailMessage.To.Add(new MailboxAddress("Recipient", emailRecord.Email));
                }
                emailMessage.Subject = subject;
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
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
                        </table>"
                };

                emailMessage.Body = bodyBuilder.ToMessageBody();

                // Use the helper method for reliable email sending
                bool emailSent = await SendEmailWithFallbackAsync(emailMessage);

                if (!emailSent)
                {
                    throw new Exception("All SMTP configurations failed. Please check your email configuration.");
                }

                Console.WriteLine("Contact email process completed successfully");
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

        public string ConvertCvToBase64(IFormFile cvFile)
        {
            if (cvFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    cvFile.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(fileBytes);
                }
            }
            return null;
        }

        private async Task<bool> TrySendEmailAsync(MimeMessage emailMessage, string smtpHost, int port, SecureSocketOptions securityOption, string description)
        {
            try
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                
                // Set timeout for production reliability
                client.Timeout = 30000; // 30 seconds
                
                await client.ConnectAsync(smtpHost, port, securityOption);
                await client.AuthenticateAsync("webcmkkablo@gmail.com", "yrmmegzyzbosuoph");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
                
                Console.WriteLine($"Email sent successfully via {description}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{description} failed: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SendEmailWithFallbackAsync(MimeMessage emailMessage)
        {
            // Try multiple SMTP configurations for production reliability
            var configurations = new[]
            {
                new { Host = "smtp.gmail.com", Port = 587, Security = SecureSocketOptions.StartTls, Description = "Gmail SMTP (TLS)" },
                new { Host = "smtp.gmail.com", Port = 465, Security = SecureSocketOptions.SslOnConnect, Description = "Gmail SMTP (SSL)" }
            };

            foreach (var config in configurations)
            {
                if (await TrySendEmailAsync(emailMessage, config.Host, config.Port, config.Security, config.Description))
                {
                    return true;
                }
            }

            return false;
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
                // First, save the career information to database
                savedCareerInfo = _careerInformationRepository.CreateCareerInformation(careerInformation);
                Console.WriteLine($"Career information saved to database with ID: {savedCareerInfo.Id}");
                
                // Get manager emails for career type
                to_mails = _managerMailRepository.GetByType("career");
                
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

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("CMK KABLO", "webcmkkablo@gmail.com"));

                foreach (var emailRecord in to_mails)
                {
                    if (!string.IsNullOrEmpty(emailRecord.Email))
                    {
                        emailMessage.To.Add(new MailboxAddress("Recipient", emailRecord.Email));
                        Console.WriteLine($"Adding recipient: {emailRecord.Email}");
                    }
                }

                // Check if we have any valid recipients
                if (!emailMessage.To.Any())
                {
                    throw new Exception("No valid recipient emails found for career application.");
                }

                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
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
                        </table>"
                };

                if (attachmentFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await attachmentFile.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        bodyBuilder.Attachments.Add(attachmentFile.FileName, memoryStream.ToArray(), ContentType.Parse(attachmentFile.ContentType));
                        Console.WriteLine($"CV attachment added: {attachmentFile.FileName}");
                    }
                }

                emailMessage.Body = bodyBuilder.ToMessageBody();

                Console.WriteLine("Attempting to send email via SMTP...");
                
                // Use the helper method for reliable email sending
                bool emailSent = await SendEmailWithFallbackAsync(emailMessage);

                if (!emailSent)
                {
                    throw new Exception("All SMTP configurations failed. Please check your email configuration.");
                }

                Console.WriteLine("Career email process completed successfully");
            }
            catch (Exception ex)
            {
                // Log the error details for debugging
                Console.WriteLine($"Career email error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Log failed email attempt
                var recipients = to_mails?.Select(m => m.Email).ToList() ?? new List<string> { toEmail };
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
                
                throw new Exception($"Email gönderilirken hata oluştu: {ex.Message}");
            }
        }


    }
}
