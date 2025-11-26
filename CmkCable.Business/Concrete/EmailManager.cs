using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using CmkCable.Entities;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using System.Linq;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
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
        
        // SMTP Configuration - defaults fall back to Brevo relay
        private string SmtpServer => 
            Environment.GetEnvironmentVariable("SMTP_SERVER") ?? 
            _configuration?["Smtp:Server"] ?? 
            "smtp-relay.brevo.com";
        private List<int> _smtpPortsCache;
        private IEnumerable<int> SmtpPorts => _smtpPortsCache ??= BuildSmtpPortList();
        private int SmtpPort => SmtpPorts.FirstOrDefault();
        private string SmtpUsername => 
            Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? 
            _configuration?["Smtp:Username"] ?? 
            "9c5aec001@smtp-brevo.com";
        private string SmtpPassword => 
            Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? 
            _configuration?["Smtp:Password"] ?? 
            string.Empty; // default blank to avoid committing secrets
        private string FromEmail => 
            Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL") ?? 
            _configuration?["Smtp:FromEmail"] ?? 
            "runner@cmkkablo.com";
        private string FromName => 
            Environment.GetEnvironmentVariable("SMTP_FROM_NAME") ?? 
            _configuration?["Smtp:FromName"] ?? 
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

            // Send emails to all recipients using SMTP
            List<string> failedEmails = new List<string>();
            foreach (var emailRecord in to_emails)
            {
                if (!string.IsNullOrEmpty(emailRecord.Email))
                {
                    try
                    {
                        await SendEmailWithSmtpAsync(
                            emailRecord.Email, 
                            subject, 
                            htmlBody, 
                            plainTextBody
                        );
                        
                        Console.WriteLine($"Successfully sent offer email to {emailRecord.Email}");
                    }
                    catch (Exception emailEx)
                    {
                        failedEmails.Add($"{emailRecord.Email}: {emailEx.Message}");
                        Console.WriteLine($"[ERROR] Failed to send offer email to {emailRecord.Email}: {emailEx.Message}");
                    }
                }
            }

            if (failedEmails.Any())
            {
                var errorDetails = string.Join("; ", failedEmails);
                throw new Exception($"Some offer emails failed to send via SMTP. Details: {errorDetails}");
            }

            Console.WriteLine("Offer email process completed successfully via SMTP");
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

                // Send emails to all recipients using SMTP
                List<string> failedEmails = new List<string>();
                foreach (var emailRecord in to_mails)
                {
                    if (!string.IsNullOrEmpty(emailRecord.Email))
                    {
                        try
                        {
                            await SendEmailWithSmtpAsync(
                                emailRecord.Email, 
                                subject, 
                                htmlBody, 
                                plainTextBody
                            );
                            
                            Console.WriteLine($"Successfully sent contact email to {emailRecord.Email}");
                        }
                        catch (Exception emailEx)
                        {
                            failedEmails.Add($"{emailRecord.Email}: {emailEx.Message}");
                            Console.WriteLine($"[ERROR] Failed to send contact email to {emailRecord.Email}: {emailEx.Message}");
                        }
                    }
                }

                if (failedEmails.Any())
                {
                    var errorDetails = string.Join("; ", failedEmails);
                    throw new Exception($"Some contact emails failed to send via SMTP. Details: {errorDetails}");
                }

                Console.WriteLine("Contact email process completed successfully via SMTP");
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

        private async Task SendEmailWithSmtpAsync(string toEmail, string subject, string htmlContent, string plainTextContent = null, byte[] attachmentData = null, string attachmentName = null, string attachmentType = null)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(toEmail))
                {
                    Console.WriteLine($"[ERROR] SendEmailWithSmtpAsync: toEmail is null or empty");
                    throw new ArgumentException("toEmail is null or empty", nameof(toEmail));
                }

                if (string.IsNullOrEmpty(subject))
                {
                    Console.WriteLine($"[ERROR] SendEmailWithSmtpAsync: subject is null or empty");
                    throw new ArgumentException("subject is null or empty", nameof(subject));
                }

                if (string.IsNullOrEmpty(htmlContent))
                {
                    Console.WriteLine($"[ERROR] SendEmailWithSmtpAsync: htmlContent is null or empty");
                    throw new ArgumentException("htmlContent is null or empty", nameof(htmlContent));
                }

                // Check SMTP configuration
                Console.WriteLine($"[DEBUG] === SMTP Configuration Check ===");
                Console.WriteLine($"[DEBUG] SMTP Server: {SmtpServer}");
                Console.WriteLine($"[DEBUG] SMTP Port: {SmtpPort}");
                Console.WriteLine($"[DEBUG] SMTP Username: {SmtpUsername}");
                Console.WriteLine($"[DEBUG] FromEmail: {FromEmail}");
                Console.WriteLine($"[DEBUG] FromName: {FromName}");
                
                var smtpPorts = SmtpPorts?.ToList() ?? new List<int>();
                if (!smtpPorts.Any())
                {
                    smtpPorts.Add(587);
                }
                Console.WriteLine($"[DEBUG] SMTP Ports to try: {string.Join(", ", smtpPorts)}");

                if (string.IsNullOrEmpty(SmtpServer))
                {
                    throw new InvalidOperationException("SMTP Server is not configured");
                }

                if (string.IsNullOrEmpty(SmtpUsername) || string.IsNullOrEmpty(SmtpPassword))
                {
                    throw new InvalidOperationException("SMTP Username or Password is not configured");
                }

                if (string.IsNullOrEmpty(FromEmail))
                {
                    throw new InvalidOperationException("FROM_EMAIL is not configured");
                }
                
                // Create mail message
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(FromEmail, FromName);
                    mailMessage.To.Add(new MailAddress(toEmail));
                    mailMessage.Subject = subject;
                    mailMessage.Body = htmlContent;
                    mailMessage.IsBodyHtml = true;
                    
                    // Add plain text alternative if provided
                    if (!string.IsNullOrEmpty(plainTextContent))
                    {
                        var plainTextView = AlternateView.CreateAlternateViewFromString(plainTextContent, null, "text/plain");
                        mailMessage.AlternateViews.Add(plainTextView);
                    }
                    
                    // Add attachment if provided
                    if (attachmentData != null && !string.IsNullOrEmpty(attachmentName))
                    {
                        try
                        {
                            var attachmentStream = new MemoryStream(attachmentData);
                            var attachment = new Attachment(attachmentStream, attachmentName);
                            attachment.ContentType = new System.Net.Mime.ContentType(attachmentType ?? "application/octet-stream");
                            mailMessage.Attachments.Add(attachment);
                            Console.WriteLine($"[INFO] Attachment added: {attachmentName} ({attachmentData.Length} bytes)");
                        }
                        catch (Exception attachEx)
                        {
                            Console.WriteLine($"[WARNING] Failed to add attachment: {attachEx.Message}");
                            // Continue without attachment rather than failing completely
                        }
                    }

                    var portErrors = new List<string>();
                    Exception lastPortException = null;
                    
                    foreach (var port in smtpPorts)
                    {
                        try
                        {
                            Console.WriteLine($"[INFO] Testing network connectivity to {SmtpServer}:{port}...");
                            await EnsureSmtpConnectivityAsync(SmtpServer, port);
                            
                            Console.WriteLine($"[INFO] Preparing to send email to: {toEmail} via port {port}");
                            Console.WriteLine($"[INFO] From: {FromEmail} ({FromName})");
                            Console.WriteLine($"[INFO] Subject: {subject}");
                            Console.WriteLine($"[INFO] Has attachment: {attachmentData != null && !string.IsNullOrEmpty(attachmentName)}");
                            
                            using (var smtpClient = new SmtpClient(SmtpServer, port))
                            {
                                smtpClient.EnableSsl = true;
                                smtpClient.UseDefaultCredentials = false;
                                smtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
                                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtpClient.Timeout = 30000; // 30 seconds - should be enough for normal SMTP operations
                                
                                if (ServicePointManager.SecurityProtocol == SecurityProtocolType.SystemDefault)
                                {
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                                }
                                
                                Console.WriteLine($"[INFO] Sending email via SMTP ({SmtpServer}:{port})...");
                                Console.WriteLine($"[DEBUG] SSL Enabled: {smtpClient.EnableSsl}");
                                Console.WriteLine($"[DEBUG] Timeout: {smtpClient.Timeout}ms");
                                Console.WriteLine($"[DEBUG] Security Protocol: {ServicePointManager.SecurityProtocol}");
                                
                                var sendStartTime = DateTime.UtcNow;
                                
                                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(25)))
                                {
                                    try
                                    {
                                        await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                                        
                                        var sendDuration = (DateTime.UtcNow - sendStartTime).TotalMilliseconds;
                                        Console.WriteLine($"[SUCCESS] SMTP email sent successfully to {toEmail} via port {port} in {sendDuration:F0}ms");
                                        return;
                                    }
                                    catch (OperationCanceledException)
                                    {
                                        throw new InvalidOperationException($"SMTP send operation timed out after 25 seconds on port {port}. This usually indicates a network connectivity issue or the SMTP server is not responding.");
                                    }
                                }
                            }
                        }
                        catch (Exception portEx)
                        {
                            lastPortException = portEx;
                            var detailedMessage = BuildPortErrorMessage(SmtpServer, port, portEx);
                            portErrors.Add(detailedMessage);
                            Console.WriteLine($"[WARNING] {detailedMessage}");
                        }
                    }
                    
                    var errorSummary = string.Join(" || ", portErrors);
                    throw new InvalidOperationException($"SMTP email sending failed via all configured ports ({string.Join(", ", smtpPorts)}). Details: {errorSummary}", lastPortException);
                }
            }
            catch (ArgumentException)
            {
                // Re-throw argument exceptions as-is
                throw;
            }
            catch (InvalidOperationException)
            {
                // Re-throw invalid operation exceptions as-is
                throw;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"[ERROR] SMTP error for {toEmail}: {smtpEx.Message}");
                Console.WriteLine($"[ERROR] SMTP Status Code: {smtpEx.StatusCode}");
                Console.WriteLine($"[DEBUG] Stack trace: {smtpEx.StackTrace}");
                
                // Provide more specific error messages
                string errorMessage = smtpEx.Message;
                if (smtpEx.InnerException != null)
                {
                    errorMessage += $" Inner exception: {smtpEx.InnerException.Message}";
                    Console.WriteLine($"[DEBUG] Inner exception: {smtpEx.InnerException.Message}");
                }
                
                // Check for blacklist/IP blocking errors
                if (smtpEx.Message.Contains("blocked", StringComparison.OrdinalIgnoreCase) || 
                    smtpEx.Message.Contains("blacklist", StringComparison.OrdinalIgnoreCase) ||
                    smtpEx.Message.Contains("not allowed", StringComparison.OrdinalIgnoreCase) ||
                    smtpEx.Message.Contains("rejected", StringComparison.OrdinalIgnoreCase) ||
                    smtpEx.StatusCode == SmtpStatusCode.ClientNotPermitted ||
                    smtpEx.StatusCode == SmtpStatusCode.GeneralFailure)
                {
                    errorMessage = $"SMTP server blocked the connection. Your server IP ({GetServerIpAddress()}) may be blacklisted by Brevo. " +
                                   $"To resolve this: 1) Contact Brevo support to whitelist your IP address, " +
                                   $"2) Ensure the Brevo SMTP relay is enabled for this account, " +
                                   $"3) Confirm the sender domain is validated. Server: {SmtpServer}:{SmtpPort}";
                }
                // Check for timeout specifically
                else if (smtpEx.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase) || 
                    smtpEx.Message.Contains("timed out", StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = $"SMTP connection timeout. Please check network connectivity and firewall settings. Server: {SmtpServer}:{SmtpPort}";
                }
                // Check for authentication errors
                else if (smtpEx.StatusCode == SmtpStatusCode.GeneralFailure &&
                    smtpEx.Message.Contains("auth", StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = $"SMTP authentication failed. Please check your Brevo SMTP username ({SmtpUsername}) and transactional key. Ensure SMTP is enabled for this sender.";
                }
                
                throw new InvalidOperationException($"SMTP email sending failed: {errorMessage}", smtpEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Email error for {toEmail}: {ex.Message}");
                Console.WriteLine($"[ERROR] Exception type: {ex.GetType().Name}");
                Console.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[DEBUG] Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"[DEBUG] Inner exception type: {ex.InnerException.GetType().Name}");
                }
                
                // Check for timeout in general exceptions too
                string errorMessage = ex.Message;
                if (ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase) || 
                    ex.Message.Contains("timed out", StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = $"Email sending timeout. Please check network connectivity to {SmtpServer}:{SmtpPort} and ensure the Brevo SMTP relay is accessible.";
                }
                
                throw new InvalidOperationException($"Email sending failed: {errorMessage}", ex);
            }
        }

        private List<int> BuildSmtpPortList()
        {
            var ports = new List<int>();
            var envPorts = Environment.GetEnvironmentVariable("SMTP_PORTS");
            if (!string.IsNullOrWhiteSpace(envPorts))
            {
                ports.AddRange(ParsePortList(envPorts));
            }
            else if (!string.IsNullOrWhiteSpace(_configuration?["Smtp:Ports"]))
            {
                ports.AddRange(ParsePortList(_configuration["Smtp:Ports"]));
            }

            if (!ports.Any())
            {
                if (int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? _configuration?["Smtp:Port"], out int singlePort))
                {
                    ports.Add(singlePort);
                }
            }

            if (!ports.Any())
            {
                ports.AddRange(new[] { 587, 465, 2525 });
            }

            return ports.Where(p => p > 0 && p < 65536).Distinct().ToList();
        }

        private IEnumerable<int> ParsePortList(string portList)
        {
            return portList
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Select(p => int.TryParse(p, out int value) ? value : (int?)null)
                .Where(v => v.HasValue && v.Value > 0 && v.Value < 65536)
                .Select(v => v.Value);
        }

        private async Task EnsureSmtpConnectivityAsync(string server, int port)
        {
            try
            {
                using (var tcpClient = new System.Net.Sockets.TcpClient())
                {
                    var connectTask = tcpClient.ConnectAsync(server, port);
                    var timeoutTask = Task.Delay(5000);
                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        tcpClient.Close();
                        throw new InvalidOperationException($"Cannot connect to SMTP server {server}:{port}. Network connectivity issue or firewall blocking. Please check if the server is reachable from the container.");
                    }

                    await connectTask;
                    Console.WriteLine($"[SUCCESS] Network connectivity test passed for {server}:{port}");
                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Cannot connect to SMTP server {server}:{port}. Error: {ex.Message}. Possible causes: outbound firewall blocks port {port}, DNS issue, or Brevo IP whitelist. Please verify by running 'nc -vz {server} {port}' inside the container.", ex);
            }
        }

        private string BuildPortErrorMessage(string server, int port, Exception ex)
        {
            var baseMessage = $"{server}:{port} => {ex.GetType().Name}: {ex.Message}";
            if (ex.InnerException != null)
            {
                baseMessage += $" | Inner: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}";
            }

            if (ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
            {
                baseMessage += " | Hint: Check outbound firewall rules and ensure the SMTP host allows this IP.";
            }
            else if (ex.Message.Contains("auth", StringComparison.OrdinalIgnoreCase))
            {
                baseMessage += $" | Hint: Confirm Brevo transactional key matches username {SmtpUsername}.";
            }
            else if (ex.Message.Contains("certificate", StringComparison.OrdinalIgnoreCase))
            {
                baseMessage += " | Hint: TLS inspection or invalid cert may be blocking the handshake.";
            }

            baseMessage += $" | Server IP: {GetServerIpAddress()}";
            return baseMessage;
        }

        private string GetServerIpAddress()
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = client.GetStringAsync("https://api.ipify.org").Result;
                    return response.Trim();
                }
            }
            catch
            {
                return "Unknown";
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

                Console.WriteLine("Attempting to send email via SMTP...");
                
                // Send emails to all recipients using SMTP
                List<string> failedEmails = new List<string>();
                foreach (var emailRecord in validEmails)
                {
                    if (!string.IsNullOrEmpty(emailRecord?.Email))
                    {
                        try
                        {
                            await SendEmailWithSmtpAsync(
                                emailRecord.Email, 
                                subject, 
                                htmlBody, 
                                plainTextBody,
                                attachmentData,
                                attachmentName,
                                attachmentType
                            );
                            
                            Console.WriteLine($"Successfully sent email to {emailRecord.Email}");
                        }
                        catch (Exception emailEx)
                        {
                            var errorMsg = emailEx is InvalidOperationException ? emailEx.Message : $"Exception: {emailEx.Message}";
                            failedEmails.Add($"{emailRecord.Email}: {errorMsg}");
                            Console.WriteLine($"[ERROR] Failed to send email to {emailRecord.Email}: {errorMsg}");
                            Console.WriteLine($"[DEBUG] Exception type: {emailEx.GetType().Name}");
                            if (emailEx.InnerException != null)
                            {
                                Console.WriteLine($"[DEBUG] Inner exception: {emailEx.InnerException.Message}");
                            }
                        }
                    }
                }

                if (failedEmails.Any())
                {
                    var errorDetails = string.Join("; ", failedEmails);
                    throw new Exception($"Some emails failed to send via SMTP. Details: {errorDetails}");
                }

                Console.WriteLine("Career email process completed successfully via SMTP");
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
