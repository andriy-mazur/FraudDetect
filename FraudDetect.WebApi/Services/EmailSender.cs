namespace FraudDetect.WebApi.Services
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using FraudDetect.Interface;
    using FraudDetect.Interface.Services;

    public class EmailSender : IEmailSender
    {
        private readonly IDbLog dbLog;

        public EmailSender(IDbLog dbLog)
        {
            this.dbLog = dbLog;
        }

        public void SendEmail(string fromAddress, string toAddress, string subject, string body)
        {
            SendEmail(fromAddress, toAddress, string.Empty, string.Empty, subject, body);
        }

        public void SendEmail(string fromAddress, string toAddress, string ccAddress, string subject, string body)
        {
            SendEmail(fromAddress, toAddress, ccAddress, string.Empty, subject, body);
        }

        public async void SendEmail(
            string fromAddress,
            string toAddress,
            string ccAddress,
            string bccAddress,
            string subject,
            string body)
        {
            dbLog.LogAsync(
                $"{{\"From\":\"{fromAddress}\", \"To\":\"{toAddress}\", \"cc\":\"{ccAddress}\", \"bcc\":\"{bccAddress}\", \"subject\":\"{subject}\", \"body\":\"{body}\"}}",
                SourceType.Email);

            var smtp = new SmtpClient
            {
                Host = "smtp-relay.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, "z#7vkrl1a1Dc")
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            try
            {
                await smtp.SendMailAsync(message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                dbLog.LogAsync($"Error on send email:{e.Message}", SourceType.Email);
            }
        }
    }
}