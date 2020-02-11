namespace FraudDetect.Interface.Services
{
    public interface IEmailSender
    {
        void SendEmail(
            string fromAddress,
            string toAddress,
            string subject,
            string body);
        
        void SendEmail(
            string fromAddress,
            string toAddress,
            string ccAddress,
            string subject,
            string body);
        
        void SendEmail(
            string fromAddress,
            string toAddress,
            string ccAddress,
            string bccAddress,
            string subject,
            string body);
    }
}