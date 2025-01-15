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

namespace CmkCable.Business.Concrete
{
    public class EmailManager
    {
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CMK KABLO", "muhammedfthyzgl@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("fyuzgul@cmkkablo.com", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("muhammedfthyzgl@gmail.com", "fplflbpsyemswkoo");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        public async Task SendCareerEmailAsync(string toEmail, string subject, string message, IFormFile attachmentFile)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CMK KABLO", "muhammedfthyzgl@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("Recipient", toEmail)); // 'Recipient' kısmını isteğe göre değiştirin
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message // HTML içeriğini burada ayarlıyoruz
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

        public async Task SendOfferEmailAsync(string toEmail, string subject, GetOffer offerDetails)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CMK KABLO", "muhammedfthyzgl@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("Recipient", toEmail));
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
    }
}
