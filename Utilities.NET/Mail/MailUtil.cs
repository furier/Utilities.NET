using System.Net.Mail;

// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Mail
{
    /// <summary> Mail utility. </summary>
    public static class MailUtil
    {
        /// <summary> The port. </summary>
        public static int Port = 25;

        /// <summary> The SMTP delivery method. </summary>
        public static SmtpDeliveryMethod SmtpDeliveryMethod = SmtpDeliveryMethod.Network;

        /// <summary> The use default credentials. </summary>
        public static bool UseDefaultCredentials = false;

        /// <summary> The host. </summary>
        public static string Host = "SMTP.GOOGLE.COM";

        /// <summary> The enable ssl. </summary>
        public static bool EnableSSL = false;

        /// <summary> Sends a mail. </summary>
        /// <param name="from"> Source for the mail. </param>
        /// <param name="to"> Destination for the mail. </param>
        /// <param name="subject"> The subject. </param>
        /// <param name="body"> The body. </param>
        public static void SendMailAsync(string from, string to, string subject, string body)
        {
            var mail = new MailMessage(from, to) { Subject = subject, Body = body };
            var client = new SmtpClient { Port = Port, DeliveryMethod = SmtpDeliveryMethod, UseDefaultCredentials = UseDefaultCredentials, Host = Host, EnableSsl = EnableSSL };
            client.SendAsync(mail, null);
        }
    }
}