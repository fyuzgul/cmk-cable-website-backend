using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using CmkCable.Entities;
using System.Net.Mail;
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
                        <tr>
                            <th>Alan</th>
                            <th>Değer</th>
                        </tr>
                        <tr>
                            <td>Ad</td>
                            <td>{offerDetails.FirstName}</td>
                        </tr>
                        <tr>
                            <td>Soyad</td>
                            <td>{offerDetails.LastName}</td>
                        </tr>
                        <tr>
                            <td>Work Email</td>
                            <td>{offerDetails.WorkEmail}</td>
                        </tr>
                        <tr>
                            <td>Rol</td>
                            <td>{offerDetails.Role?.Name}</td>
                        </tr>
                        <tr>
                            <td>Ülke</td>
                            <td>{offerDetails.Country}</td>
                        </tr>
                        <tr>
                            <td>Şirket</td>
                            <td>{offerDetails.Company}</td>
                        </tr>
                        <tr>
                            <td>Şirket Türü</td>
                            <td>{offerDetails.CompanyType?.Name}</td>
                        </tr>
                        <tr>
                            <td>Telefon</td>
                            <td>{offerDetails.TelephoneNumber}</td>
                        </tr>
                        <tr>
                            <td>Yardım Türü</td>
                            <td>{offerDetails.HelpType?.Name}</td>
                        </tr>
                        <tr>
                            <td>Mesaj</td>
                            <td>{offerDetails.Message}</td>
                        </tr>
                        <tr>
                            <td>IP Adresi</td>
                            <td>{offerDetails.IpAddress ?? "Belirtilmemiş"}</td>
                        </tr>
                        <tr>
                            <td>Açık Rıza</td>
                            <td>{(offerDetails.AcikRiza ? "Evet" : "Hayır")}</td>
                        </tr>
                        <tr>
                            <td>Oluşturulma Tarihi</td>
                            <td>{offerDetails.CreatedAt:dd/MM/yyyy HH:mm}</td>
                        </tr>
                    </table>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            try
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("webcmkkablo@gmail.com", "yrmmegzyzbosuoph");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Hata mesajını loglara yazdır
                Console.WriteLine($"Mail gönderim hatası: {ex.Message}");
            }
        }

        public async Task SendEmailAsync(string subject, ContactRequest message)
        {
            _contactRequestRepository.Add(message);
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
                        <tr><td>Adres</td><td>{message.Street}</td></tr>
                        <tr><td>Şehir</td><td>{message.City}</td></tr>
                        <tr><td>Posta Kodu</td><td>{message.Postcode}</td></tr>
                        <tr><td>Telefon</td><td>{message.TelephoneNumber}</td></tr>
                        <tr><td>Email</td><td>{message.Email}</td></tr>
                        <tr><td>Mesaj</td><td>{message.Message}</td></tr>
                        <tr><td>IP Adresi</td><td>{message.IpAddress}</td></tr>
                        <tr><td>Açık Rıza</td><td>{(message.Consent ? "Evet" : "Hayır")}</td></tr>
                        <tr><td>Oluşturulma Tarihi</td><td>{message.CreatedAt:dd/MM/yyyy HH:mm}</td></tr>
                    </table>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            try
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("webcmkkablo@gmail.com", "yrmmegzyzbosuoph");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Hata mesajını loglara yazdır
                Console.WriteLine($"Mail gönderim hatası: {ex.Message}");
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


        public async Task SendCareerEmailAsync(string toEmail, string subject, CareerInformation careerInformation, IFormFile attachmentFile)
        {
            try
            {
                // First, save the career information to database
                _careerInformationRepository.CreateCareerInformation(careerInformation);
                
                // Get manager emails for career type
                var to_mails = _managerMailRepository.GetByType("career");
                
                // Check if any manager emails are found
                if (to_mails == null || !to_mails.Any())
                {
                    // Fallback to the provided email if no manager emails are configured
                    if (!string.IsNullOrEmpty(toEmail))
                    {
                        to_mails = new List<ManagerMail> { new ManagerMail { Email = toEmail } };
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
                    }
                }

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("webcmkkablo@gmail.com", "yrmmegzyzbosuoph");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the error details for debugging
                Console.WriteLine($"Career email error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Email gönderilirken hata oluştu: {ex.Message}");
            }
        }


    }
}
