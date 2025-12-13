using System.Net;
using System.Net.Mail;


namespace FinalAppTemplate.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationEmailAsync(string toEmail, string verifyUrl)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var fromEmail = emailSettings["FromEmail"];
            var password = emailSettings["AppPassword"];
            var host = emailSettings["SmtpServer"];
            var port = int.Parse(emailSettings["Port"]);

            var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Verify your account",
                // CHANGE THIS LINE:
                Body = $@"
                    <h2>Welcome!</h2>
                    <p>Please click <a href='{verifyUrl}'>here</a> to verify your email.</p>
                    <br/>
                    <p><strong>If the link above is not clickable, copy and paste this URL into your browser:</strong></p>
                    <p>{verifyUrl}</p>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
