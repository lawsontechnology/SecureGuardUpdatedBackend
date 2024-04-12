using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.IO;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Services;

namespace Visitor_Management_System.Infrastructure.Email
{
    public class EmailSender : IMailServices
    {
        public void SendEMail(EMailDto mailRequest)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 465;
            string username = "secureguard77@gmail.com";
            string password = "stzb ztka ernn pdpo";

            string senderEmail = "secureguard77@gmail.com";
            string recipientEmail = mailRequest.ToEmail;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Secure Guard", senderEmail));
            message.To.Add(new MailboxAddress(mailRequest.ToName, recipientEmail));
            message.Subject = mailRequest.Subject;

            var body = new TextPart("html")
            {
                Text = mailRequest.HtmlContent,
            };
            message.Body = body;

            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, smtpPort, true);
                Console.WriteLine("Entered");
                client.Authenticate(username, password);

                client.Send(message);

                client.Disconnect(true);
            }
        }

        public void QRCodeEMail(EMailDto mailRequest, string qrCodeImagePath)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 465;
            string username = "secureguard77@gmail.com";
            string password = "stzb ztka ernn pdpo";

            string senderEmail = "secureguard77@gmail.com";
            string recipientEmail = mailRequest.ToEmail;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Secure Guard", senderEmail));
            message.To.Add(new MailboxAddress(mailRequest.ToName, recipientEmail));
            message.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.HtmlContent;

            // Add the QR code image as an attachment
            var attachment = builder.Attachments.Add(qrCodeImagePath);
            attachment.ContentId = "qrcode";

            // Set the message body
            message.Body = builder.ToMessageBody();


            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, smtpPort, true);
                client.Authenticate(username, password);

                client.Send(message);

                client.Disconnect(true);
            }
        }



    }
}
