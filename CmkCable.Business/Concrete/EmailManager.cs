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
            _getOfferRepository.CreateGetOffer(offerDetails);

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
                        <tr><td>Ad Soyad</td><td>{offerDetails.AdSoyad}</td></tr>
                        <tr><td>Firma Adı</td><td>{offerDetails.FirmaAdi}</td></tr>
                        <tr><td>Telefon</td><td>{offerDetails.Telefon}</td></tr>
                        <tr><td>Email</td><td>{offerDetails.Email}</td></tr>
                        <tr><td>Ülke</td><td>{offerDetails.Ulke}</td></tr>
                        <tr><td>Kablolar</td><td>{offerDetails.Kablolar}</td></tr>
                        <tr><td>Açıklama</td><td>{offerDetails.Aciklama}</td></tr>
                        <tr><td>LME</td><td>{offerDetails.Lme}</td></tr>
                        <tr><td>Para Birimleri</td><td>{string.Join(", ", offerDetails.ParaBirimleri)}</td></tr>
                        <tr><td>Teslim Şekli</td><td>{offerDetails.TeslimSekli}</td></tr>
                        <tr><td>Teslim Yeri</td><td>{offerDetails.TeslimYeri}</td></tr>
                        <tr><td>Ödeme Şekli</td><td>{offerDetails.OdemeSekli}</td></tr>
                        <tr><td>Ambalajlama</td><td>{offerDetails.Ambalajlama}</td></tr>
                        <tr><td>IP Adresi</td><td>{offerDetails.IpAddress}</td></tr>

                        <tr><td>Açık Rıza</td><td>{(offerDetails.AcikRiza ? "Evet" : "Hayır")}</td></tr>
                        <tr><td>Oluşturulma Tarihi</td><td>{offerDetails.CreatedAt:dd/MM/yyyy HH:mm}</td></tr>
                    </table>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("webcmkkablo@gmail.com", "fqwlquybhjbwwtit");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string subject, ContactRequest message)
        {
            _contactRequestRepository.CreateContactRequest(message);
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


            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("webcmkkablo@gmail.com", "fqwlquybhjbwwtit");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
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


        public async Task SendCareerEmailAsync(string toEmail, string subject, CareerInformation careerInformation, List<Experience> experiences, IFormFile attachmentFile)
        {
            _careerInformationRepository.CreateCareerInformation(careerInformation, experiences);
            var to_mails = _managerMailRepository.GetByType("career");
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
                    <h1>Kariyer Detayları</h1>
                    <table>
                        <tr><th>Alan</th><th>Değer</th></tr>
                        <tr><td>Ad Soyad</td><td>{careerInformation.FullName}</td></tr>
                        <tr><td>Email</td><td>{careerInformation.Email}</td></tr>
                        <tr><td>Cinsiyet</td><td>{careerInformation.Gender}</td></tr>
                        <tr><td>Medeni Durum</td><td>{careerInformation.MaritalStatus}</td></tr>
                        <tr><td>Askerlik Durumu</td><td>{careerInformation.MilitaryStatus}</td></tr>
                        <tr><td>Sürücü Belgesi</td><td>{careerInformation.DriverLicense}</td></tr>
                        <tr><td>Seyahat Durumu</td><td>{careerInformation.TravelAvailability}</td></tr>
                        <tr><td>Okul</td><td>{careerInformation.School}</td></tr>
                        <tr><td>Fakülte</td><td>{careerInformation.Faculty}</td></tr>
                        <tr><td>Mezuniyet Tarihi</td><td>{careerInformation.GraduationDate}</td></tr>
                        <tr><td>Diller</td><td>{careerInformation.Languages}</td></tr>
                        <tr><td>Yazılım Bilgisi</td><td>{careerInformation.SoftwareSkills}</td></tr>
                        <tr><td>Seminerler</td><td>{careerInformation.Seminars}</td></tr>
                        <tr><td>Bölüm</td><td>{careerInformation.Department}</td></tr>
                        <tr><td>Referans Kaynağı</td><td>{careerInformation.ReferenceSource}</td></tr>
                        <tr><td>Açıklama</td><td>{careerInformation.Description}</td></tr>
                        <tr><td>CV</td><td>{careerInformation.Cv?.FileName}</td></tr>
                        <tr><td>IP Adresi</td><td>{careerInformation.IpAddress}</td></tr>
                        <tr><td>Açık Rıza</td><td>{(careerInformation.Consent ? "Evet" : "Hayır")}</td></tr>
                        <tr><td>Oluşturulma Tarihi</td><td>{careerInformation.CreatedAt:dd/MM/yyyy HH:mm}</td></tr>
                    </table>
                    <h2>Deneyimler</h2>
                    <table>
                        <tr>
                            <th>Şirket</th>
                            <th>Süre</th>
                            <th>Pozisyon</th>
                        </tr>
                        {string.Join("", experiences.Select(e => $@"
                            <tr>
                                <td>{e.Company}</td>
                                <td>{e.Duration}</td>
                                <td>{e.Position}</td>
                            </tr>
                        "))}
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
            await client.AuthenticateAsync("webcmkkablo@gmail.com", "fqwlquybhjbwwtit");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }


    }
}
