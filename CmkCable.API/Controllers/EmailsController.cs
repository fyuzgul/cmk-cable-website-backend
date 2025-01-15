using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private EmailManager _emailManager;
        public EmailsController() { _emailManager = new EmailManager(); }
        [HttpPost("send-offer")]
        public async Task<IActionResult> SendOffer([FromForm] GetOffer offerDetails)
        {
            if (offerDetails == null)
            {
                return BadRequest("Teklif detayları boş olamaz.");
            }

            try
            {
                await _emailManager.SendOfferEmailAsync(
                    "fyuzgul@cmkkablo.com",
                    "Yeni Teklif Talebi",
                    offerDetails
                );

                return Ok("Teklif başarıyla gönderildi.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"E-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] ContactRequest contactRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = $"New message from {contactRequest.FullName}";
            var message = $"Name: {contactRequest.FullName}\n" +
                          $"Email: {contactRequest.Email}\n" +
                          $"Message: {contactRequest.Message}\n" +
                          $"City: {contactRequest.City}\n" +
                          $"Postcode: {contactRequest.Postcode}\n" +
                          $"Telephone Number: {contactRequest.TelephoneNumber}\n" +
                          $"Street:{contactRequest.Street}\n";


            await _emailManager.SendEmailAsync("fyuzgul@cmkkablo.com", subject, message);
            return Ok(new { message = "Email sent successfully" });
        }

        [HttpPost("career-email")]
        public async Task<IActionResult> SubmitCareerForm([FromForm] CareerInformation model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = $"New message from {model.FullName}";
            var message = $"<html>" +
              $"<body style='font-family: Arial, sans-serif;'>" +
              $"<h2 style='color: #4A90E2;'>Açıklama</h2>" +
              $"<p style='font-size: 16px;'>" +
              $"<strong style='color: #333;'>Departman:</strong> <span style='color: #555;'>{model.Department}</span><br>" +
              $"<strong style='color: #333;'>Cinsiyet:</strong> <span style='color: #555;'>{model.Gender}</span><br>" +
            $"<strong style='color: #333;'>Okul:</strong> <span style='color: #555;'>{model.Education.School}</span><br>" +
            $"<strong style='color: #333;'>Fakülte:</strong> <span style='color: #555;'>{model.Education.Faculty}</span><br>" +
            $"<strong style='color: #333;'>Mezuniyet Tarihi:</strong> <span style='color: #555;'>{model.Education.GraduationDate}</span><br>" +
            $"<strong style='color: #333;'>Medeni Durum:</strong> <span style='color: #555;'>{model.MaritalStatus}</span><br>" +
            $"<strong style='color: #333;'>Sürücü Belgesi:</strong> <span style='color: #555;'>{model.DriverLicense}</span><br>" +
            $"<strong style='color: #333;'>Referans Kaynağı:</strong> <span style='color: #555;'>{model.ReferenceSource}</span><br>" +
            $"<strong style='color: #333;'>Telefon Numarası:</strong> <span style='color: #555;'>{model.TelephoneNumber}</span><br>" +
            $"<strong style='color: #333;'>Açıklama:</strong> <span style='color: #555;'>{model.Description}</span><br>" +
            $"<strong style='color: #333;'>Onay:</strong> <span style='color: #555;'>{model.Consent}</span><br>" +
              $"</p>" +
              $"<footer style='font-size: 12px; color: #888;'>Bu e-posta içeriği gizlidir ve yalnızca belirtilen alıcı içindir.</footer>" +
              $"</body>" +
              $"</html>";


            var cvFile = model.Cv;

            await _emailManager.SendCareerEmailAsync("fyuzgul@cmkkablo.com", subject, message, cvFile);
            return Ok(new { message = "Email sent successfully" });
        }

    }
}
