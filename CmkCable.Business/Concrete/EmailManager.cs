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
            emailMessage.From.Add(new MailboxAddress("CMK KABLO", "muhammedfthyzgl@gmail.com"));

            foreach (var emailRecord in to_emails)
            {
                emailMessage.To.Add(new MailboxAddress("Recipient", emailRecord.Email));
            }

            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>Teklif Detayları</h1>" +
                           $"<p><strong>Ad Soyad:</strong> {offerDetails.AdSoyad}</p>" +
                           $"<p><strong>Firma Adı:</strong> {offerDetails.FirmaAdi}</p>" +
                           $"<p><strong>Telefon:</strong> {offerDetails.Telefon}</p>" +
                           $"<p><strong>Email:</strong> {offerDetails.Email}</p>" +
                           $"<p><strong>Ülke:</strong> {offerDetails.Ulke}</p>" +
                           $"<p><strong>Kablolar:</strong> {offerDetails.Kablolar}</p>" +
                           $"<p><strong>Açıklama:</strong> {offerDetails.Aciklama}</p>" +
                           $"<p><strong>LME:</strong> {offerDetails.Lme}</p>" +
                           $"<p><strong>Para Birimleri:</strong> {string.Join(", ", offerDetails.ParaBirimleri)}</p>" +
                           $"<p><strong>Teslim Şekli:</strong> {offerDetails.TeslimSekli}</p>" +
                           $"<p><strong>Teslim Yeri:</strong> {offerDetails.TeslimYeri}</p>" +
                           $"<p><strong>Ödeme Şekli:</strong> {offerDetails.OdemeSekli}</p>" +
                           $"<p><strong>Ambalajlama:</strong> {offerDetails.Ambalajlama}</p>" +
                           $"<p><strong>Açık Rıza:</strong> {(offerDetails.AcikRiza ? "Evet" : "Hayır")}</p>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("muhammedfthyzgl@gmail.com", "fplflbpsyemswkoo");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailAsync( string subject, ContactRequest message)
        {
            _contactRequestRepository.CreateContactRequest(message);
            var to_mails = _managerMailRepository.GetByType("contact");
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CMK KABLO", "muhammedfthyzgl@gmail.com"));

            foreach (var emailRecord in to_mails)
            {
                emailMessage.To.Add(new MailboxAddress("Recipient", emailRecord.Email));
            }
            emailMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>İletişim Detayları</h1>" +
                           $"<p><strong>Ad Soyad:</strong> {message.FullName}</p>" +
                           $"<p><strong>Adres:</strong> {message.Street}</p>" +
                           $"<p><strong>Posta Kodu:</strong> {message.Postcode}</p>" +
                           $"<p><strong>Telefon:</strong> {message.TelephoneNumber}</p>" +
                           $"<p><strong>Mail:</strong> {message.Email}</p>" +
                           $"<p><strong>Mesaj:</strong> {message.Message}</p>" +
                           $"<p><strong>Açık Rıza:</strong> {(message.Consent ? "Evet" : "Hayır")}</p>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();


            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("muhammedfthyzgl@gmail.com", "fplflbpsyemswkoo");
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
            emailMessage.From.Add(new MailboxAddress("CMK KABLO", "muhammedfthyzgl@gmail.com"));

            foreach (var emailRecord in to_mails)
            {
                emailMessage.To.Add(new MailboxAddress("Recipient", emailRecord.Email));
            }
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>Kariyer Detayları</h1>" +
           $"<p><strong>Ad Soyad:</strong> {careerInformation.FullName}</p>" +
           $"<p><strong>Telefon:</strong> {careerInformation.TelephoneNumber}</p>" +
           $"<p><strong>Email:</strong> {careerInformation.Email}</p>" +
           $"<p><strong>Cinsiyet:</strong> {careerInformation.Gender}</p>" +
           $"<p><strong>Medeni Durum:</strong> {careerInformation.MaritalStatus}</p>" +
           $"<p><strong>Askerlik Durumu:</strong> {careerInformation.MilitaryStatus}</p>" +
           $"<p><strong>Sürücü Belgesi:</strong> {careerInformation.DriverLicense}</p>" +
           $"<p><strong>Seyahat Durumu:</strong> {careerInformation.TravelAvailability}</p>" +
           $"<p><strong>Okulu:</strong> {careerInformation.School?.ToString()}</p>" + // Assuming Education has a ToString() method or you can format it accordingly
           $"<p><strong>Fakülte:</strong> {careerInformation.Faculty}</p>" +
           $"<p><strong>Mezuniyet Tarihi:</strong> {careerInformation.GraduationDate}</p>" +
           $"<p><strong>Diller:</strong> {careerInformation.Languages}</p>" +
           $"<p><strong>Yazılım Bilgisi:</strong> {careerInformation.SoftwareSkills}</p>" +
           $"<p><strong>Seminerler:</strong> {careerInformation.Seminars}</p>" +
           $"<p><strong>Deneyimler:</strong> {string.Join(", ", experiences.Select(e => e.ToString()))}</p>" + // Assuming Experience has a ToString() method
           $"<p><strong>Bölüm:</strong> {careerInformation.Department}</p>" +
           $"<p><strong>Referans Kaynağı:</strong> {careerInformation.ReferenceSource}</p>" +
           $"<p><strong>Açıklama:</strong> {careerInformation.Description}</p>" +
           $"<p><strong>CV:</strong> {careerInformation.Cv?.FileName}</p>" + // Assuming Cv is an IFormFile, you may want to display the filename
           $"<p><strong>Açık Rıza:</strong> {(careerInformation.Consent ? "Evet" : "Hayır")}</p>" +
           $"<p><strong>Oluşturulma Tarihi:</strong> {careerInformation.CreatedAt}</p>"

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
            await client.AuthenticateAsync("muhammedfthyzgl@gmail.com", "fplflbpsyemswkoo");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }


    }
}
