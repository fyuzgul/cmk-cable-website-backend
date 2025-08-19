using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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

        
        [HttpPost("submit-career-form")]
        public async Task<IActionResult> SubmitCareerForm([FromForm] CareerInformation model)
        {
            try
            {
                Console.WriteLine("=== SubmitCareerForm called ===");
                Console.WriteLine($"Model received: {model != null}");
                Console.WriteLine($"Full name: {model?.FullName ?? "NULL"}");
                Console.WriteLine($"Email: {model?.Email ?? "NULL"}");
                Console.WriteLine($"Telephone: {model?.TelephoneNumber ?? "NULL"}");
                Console.WriteLine($"CV file: {model?.Cv?.FileName ?? "No CV"}");
                Console.WriteLine($"CV file size: {model?.Cv?.Length ?? 0} bytes");
                Console.WriteLine($"CV content type: {model?.Cv?.ContentType ?? "NULL"}");

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
                    Console.WriteLine("Full name validation failed");
                    return BadRequest(new { message = "Full name is required" });
                }

                if (string.IsNullOrEmpty(model.Email))
                {
                    Console.WriteLine("Email validation failed");
                    return BadRequest(new { message = "Email is required" });
                }

                // Validate email format
                try
                {
                    var email = new System.Net.Mail.MailAddress(model.Email);
                    Console.WriteLine($"Email format validated: {email.Address}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Email format validation failed");
                    return BadRequest(new { message = "Invalid email format" });
                }

                // Set default values for missing fields
                if (model.CreatedAt == default)
                {
                    model.CreatedAt = DateTime.UtcNow;
                    Console.WriteLine($"Set CreatedAt to: {model.CreatedAt}");
                }

                // Set IP address if not provided
                if (string.IsNullOrEmpty(model.IpAddress))
                {
                    model.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                    Console.WriteLine($"Set IP address to: {model.IpAddress}");
                }

                Console.WriteLine("All validations passed, attempting to send career email...");
                
                try
                {
                    await _emailManager.SendCareerEmailAsync("fyuzgul@cmkkablo.com", "Kariyer", model, model.Cv);
                    Console.WriteLine("Career email sent successfully");
                    
                    return Ok(new { message = "Career application submitted successfully" });
                }
                catch (Exception emailEx)
                {
                    Console.WriteLine($"Email sending failed: {emailEx.Message}");
                    Console.WriteLine($"Email exception stack trace: {emailEx.StackTrace}");
                    
                    // Return a specific error for email failures
                    return StatusCode(500, new { 
                        message = "Career application received but email notification failed. Please contact support.",
                        error = emailEx.Message 
                    });
                }
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

        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail()
        {
            try
            {
                Console.WriteLine("=== TestEmail called ===");
                
                // Test basic email sending without attachment
                var testCareerInfo = new CareerInformation
                {
                    FullName = "Test User",
                    Email = "test@example.com",
                    TelephoneNumber = "1234567890",
                    Consent = true,
                    CreatedAt = DateTime.UtcNow,
                    IpAddress = "127.0.0.1"
                };

                Console.WriteLine("Sending test email...");
                await _emailManager.SendCareerEmailAsync("fyuzgul@cmkkablo.com", "Test Email", testCareerInfo, null);
                
                return Ok(new { message = "Test email sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test email failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                return StatusCode(500, new { 
                    message = "Test email failed",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
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
        public IActionResult HealthCheck()
        {
            try
            {
                Console.WriteLine("=== HealthCheck called ===");
                
                using (var context = new CmkCable.DataAccess.CmkCableDbContext())
                {
                    // Test database connection
                    var canConnect = context.Database.CanConnect();
                    Console.WriteLine($"Database connection: {canConnect}");
                    
                    if (!canConnect)
                    {
                        return StatusCode(500, new { 
                            status = "unhealthy",
                            database = "disconnected",
                            message = "Cannot connect to database"
                        });
                    }

                    // Test basic queries
                    var formTypesCount = context.FormTypes.Count();
                    var managerMailsCount = context.ManagerMails.Count();
                    var mailFormTypesCount = context.MailFormTypes.Count();
                    var careerInfoCount = context.CareerInformations.Count();
                    
                    Console.WriteLine($"FormTypes: {formTypesCount}");
                    Console.WriteLine($"ManagerMails: {managerMailsCount}");
                    Console.WriteLine($"MailFormTypes: {mailFormTypesCount}");
                    Console.WriteLine($"CareerInformations: {careerInfoCount}");

                    // Check if required data exists
                    var careerFormType = context.FormTypes.FirstOrDefault(ft => ft.FormTypes == "career");
                    var careerManagerMails = context.MailFormTypes
                        .Where(mft => mft.FormTypeId == careerFormType.Id)
                        .Select(mft => mft.MailId)
                        .ToList();

                    var healthStatus = new
                    {
                        status = "healthy",
                        database = "connected",
                        formTypes = formTypesCount,
                        managerMails = managerMailsCount,
                        mailFormTypes = mailFormTypesCount,
                        careerInfo = careerInfoCount,
                        careerFormTypeExists = careerFormType != null,
                        careerManagerMailsCount = careerManagerMails.Count,
                        timestamp = DateTime.UtcNow
                    };

                    Console.WriteLine($"Health check completed: {healthStatus.status}");
                    return Ok(healthStatus);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Health check failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                return StatusCode(500, new { 
                    status = "unhealthy",
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
