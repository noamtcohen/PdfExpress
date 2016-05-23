using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using MailMessage = System.Net.Mail.MailMessage;

namespace PdfExpress
{
    public class Emailer
    {
        private string _host;
        private int _port;
        private string _username;
        private string _passport;
        private readonly List<string> _attachments = new List<string>();
        public void SetSmtpParams(string host, int port, string username, string passport)
        {
            _passport = passport;
            _username = username;
            _port = port;
            _host = host;
        }

        public void AddAttachment(string path)
        {
            _attachments.Add(path);
        }

        public bool SendEmail(string from, string to, string subject, string body)
        {
            var mail = new MailMessage(from, to, subject, body);

            foreach (var attachment in _attachments)
                mail.Attachments.Add(new Attachment(attachment));

            var alternameView = AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html"));
            mail.AlternateViews.Add(alternameView);

            var smtpClient = new SmtpClient(_host, _port) {Credentials = new NetworkCredential(_username, _passport)};
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}