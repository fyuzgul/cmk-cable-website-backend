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
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

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
            try
            {
                // Log the incoming request
                Console.WriteLine($"Career form submission received for: {model?.FullName ?? "Unknown"}");
                Console.WriteLine($"Email: {model?.Email ?? "Unknown"}");
                Console.WriteLine($"CV file: {model?.Cv?.FileName ?? "No CV"}");

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    Console.WriteLine($"Model validation failed: {errors}");
                    return BadRequest(new { message = "Form validation failed", errors = errors });
                }

                if (model == null)
                {
                    Console.WriteLine("Career form model is null");
                    return BadRequest(new { message = "Career form data is required" });
                }

                // Validate required fields
                if (string.IsNullOrEmpty(model.FullName))
                {
                    return BadRequest(new { message = "Full name is required" });
                }

                if (string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                // Set default values for missing fields
                if (model.CreatedAt == default)
                {
                    model.CreatedAt = DateTime.UtcNow;
                }

                Console.WriteLine("Attempting to send career email...");
                await _emailManager.SendCareerEmailAsync("fyuzgul@cmkkablo.com", "Kariyer", model, model.Cv);
                Console.WriteLine("Career email sent successfully");
                
                return Ok(new { message = "Career application submitted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SubmitCareerForm: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Return a more user-friendly error message
                return StatusCode(500, new { 
                    message = "Career application submission failed. Please try again later.",
                    error = ex.Message 
                });
            }
        }

        [HttpGet("debug-config")]
        public IActionResult GetEmailConfiguration()
        {
            try
            {
                using (var context = new CmkCable.DataAccess.CmkCableDbContext())
                {
                    var formTypes = context.FormTypes.ToList();
                    var managerMails = context.ManagerMails.ToList();
                    var mailFormTypes = context.MailFormTypes
                        .Include(mft => mft.FormType)
                        .Include(mft => mft.ManagerMail)
                        .ToList();

                    var config = new
                    {
                        FormTypes = formTypes.Select(ft => new { ft.Id, ft.FormTypes }),
                        ManagerMails = managerMails.Select(mm => new { mm.Id, mm.Email }),
                        MailFormTypeRelations = mailFormTypes.Select(mft => new
                        {
                            mft.Id,
                            FormType = mft.FormType?.FormTypes,
                            ManagerMail = mft.ManagerMail?.Email
                        })
                    };

                    return Ok(config);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("seed-data")]
        public IActionResult SeedEmailData()
        {
            try
            {
                using (var context = new CmkCable.DataAccess.CmkCableDbContext())
                {
                    // Check if FormTypes exist, if not create them
                    if (!context.FormTypes.Any())
                    {
                        var formTypes = new List<CmkCable.Entities.FormType>
                        {
                            new CmkCable.Entities.FormType { FormTypes = "career" },
                            new CmkCable.Entities.FormType { FormTypes = "offer" },
                            new CmkCable.Entities.FormType { FormTypes = "contact" }
                        };
                        context.FormTypes.AddRange(formTypes);
                        context.SaveChanges();
                    }

                    // Check if ManagerMails exist for career type, if not create default
                    var careerFormType = context.FormTypes.FirstOrDefault(ft => ft.FormTypes == "career");
                    if (careerFormType != null)
                    {
                        var existingCareerMails = context.MailFormTypes
                            .Where(mft => mft.FormTypeId == careerFormType.Id)
                            .Select(mft => mft.MailId)
                            .ToList();

                        if (!existingCareerMails.Any())
                        {
                            // Create default manager mail for career
                            var defaultManagerMail = new CmkCable.Entities.ManagerMail { Email = "fyuzgul@cmkkablo.com" };
                            context.ManagerMails.Add(defaultManagerMail);
                            context.SaveChanges();

                            // Create MailFormType relationship
                            var mailFormType = new CmkCable.Entities.MailFormType
                            {
                                MailId = defaultManagerMail.Id,
                                FormTypeId = careerFormType.Id
                            };
                            context.MailFormTypes.Add(mailFormType);
                            context.SaveChanges();
                        }
                    }

                    return Ok(new { message = "Email data seeded successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("health-check")]
        public async Task<IActionResult> EmailHealthCheck()
        {
            try
            {
                var testMessage = new MimeMessage();
                testMessage.From.Add(new MailboxAddress("CMK KABLO", "webcmkkablo@gmail.com"));
                testMessage.To.Add(new MailboxAddress("Test", "test@example.com"));
                testMessage.Subject = "Email Health Check";
                testMessage.Body = new TextPart("plain") { Text = "This is a test email to check SMTP connectivity." };

                var emailManager = new EmailManager();
                
                // Test SMTP connectivity without actually sending
                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Timeout = 10000; // 10 seconds for health check
                
                var healthResults = new List<object>();
                
                // Test Gmail TLS
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("webcmkkablo@gmail.com", "yrmmegzyzbosuoph");
                    await client.DisconnectAsync(true);
                    healthResults.Add(new { Method = "Gmail SMTP (TLS)", Status = "Success", Port = 587 });
                }
                catch (Exception ex)
                {
                    healthResults.Add(new { Method = "Gmail SMTP (TLS)", Status = "Failed", Port = 587, Error = ex.Message });
                }

                // Test Gmail SSL
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync("webcmkkablo@gmail.com", "yrmmegzyzbosuoph");
                    await client.DisconnectAsync(true);
                    healthResults.Add(new { Method = "Gmail SMTP (SSL)", Status = "Success", Port = 465 });
                }
                catch (Exception ex)
                {
                    healthResults.Add(new { Method = "Gmail SMTP (SSL)", Status = "Failed", Port = 465, Error = ex.Message });
                }

                var overallStatus = healthResults.Any(r => r.GetType().GetProperty("Status")?.GetValue(r)?.ToString() == "Success") ? "Healthy" : "Unhealthy";
                
                return Ok(new { 
                    status = overallStatus,
                    timestamp = DateTime.UtcNow,
                    results = healthResults
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    status = "Error", 
                    error = ex.Message, 
                    timestamp = DateTime.UtcNow 
                });
            }
        }
    }
}
