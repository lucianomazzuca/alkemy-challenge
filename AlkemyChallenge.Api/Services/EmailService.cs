using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Services
{
    public class EmailService
    {
        public async Task SendEmail(string email)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("mazzucaluciano@hotmail.com"),
                Subject = "Bienvenido",
                PlainTextContent = "Gracias por registrarte.",
                HtmlContent = ""
            };
            msg.AddTo(new EmailAddress(email));
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
