using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

using OdinCMS.Utility;

namespace OdinCMS.Utility
{
    public class EmailSender : IEmailSender
    {



        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            // Generate mail
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(""));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = htmlMessage};

            // Send
            using (SmtpClient emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.doamin.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate(EmailSettings.UserName,"pass");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
