using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous]
        public async Task<IActionResult> SendEmail([FromBody] ContactRequest contactRequest)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"Model validation failed: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return BadRequest(ModelState);
            }
            try
            {
                Console.WriteLine($"Attempting to send email to contact request: {contactRequest.Email}");
                await _emailManager.SendEmailAsync("İletişim", contactRequest);
                Console.WriteLine("Email sent successfully");
                return Ok(new { message = "Email sent successfully" });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"E-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }

        
        [HttpPost("career-email")]
        public async Task<IActionResult> SubmitCareerForm([FromForm] CareerInformation model)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _emailManager.SendCareerEmailAsync("fyuzgul@cmkkablo.com", "Kariyer", model, model.Cv);
                return Ok(new { message = "Email sent successfully" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"E-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }

    }
}
