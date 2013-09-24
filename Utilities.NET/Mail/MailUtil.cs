#region File Header

// //////////////////////////////////////////////////////
// /// File: MailUtil.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-24 20:26
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System.Net.Mail;

#endregion

namespace Utilities.NET.Mail
{
    /// <summary>   Mail utility. </summary>
    /// <remarks>   Furier, 24.09.2013. </remarks>
    public static class MailUtil
    {
        /// <summary>   The port. </summary>
        public static int Port = 25;

        /// <summary>   The SMTP delivery method. </summary>
        public static SmtpDeliveryMethod SmtpDeliveryMethod = SmtpDeliveryMethod.Network;

        /// <summary>   The use default credentials. </summary>
        public static bool UseDefaultCredentials = false;

        /// <summary>   The host. </summary>
        public static string Host = "SMTP.GOOGLE.COM";

        /// <summary>   The enable ssl. </summary>
        public static bool EnableSSL = false;

        /// <summary>   Sends a mail. </summary>
        /// <remarks>   Furier, 24.09.2013. </remarks>
        /// <param name="from">     Source for the. </param>
        /// <param name="to">       to. </param>
        /// <param name="subject">  The subject. </param>
        /// <param name="body">     The body. </param>
        public static void SendMailAsync(string from, string to, string subject, string body)
        {
            var mail = new MailMessage(from, to) {Subject = subject, Body = body};
            var client = new SmtpClient {Port = Port, DeliveryMethod = SmtpDeliveryMethod, UseDefaultCredentials = UseDefaultCredentials, Host = Host, EnableSsl = EnableSSL};
            client.SendAsync(mail, null);
        }
    }
}