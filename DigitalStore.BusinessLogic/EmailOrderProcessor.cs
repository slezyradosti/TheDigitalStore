using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Text;
using DigitalStore.Repos.Interfaces;
using DigitalStore.BusinessLogic.Interfaces;
using DigitalStore.Models;
using DigitalStore.Models.NotForDB;

namespace DigitalStore.BusinessLogic
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", emailSettings.MailFromAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(emailSettings.ServerName, emailSettings.ServerPort, emailSettings.UseSsl);
                await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendPurchaseEmailAsync(Customer customer, string subject, Cart cart)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", emailSettings.MailFromAddress));
            emailMessage.To.Add(new MailboxAddress("", customer.Email));
            emailMessage.Subject = subject;

            StringBuilder body = new StringBuilder()
                    .AppendLine("New order processed")
                    .AppendLine("---")
                    .AppendLine("Products:");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.ProductPrice * line.Quantity;
                body.AppendFormat("{0} x {1} (total: {2:c}",
                    line.Quantity, line.Product.ProductName, subtotal);
            }

            body.AppendFormat("Total cost: {0:c}", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Shipping:")
                .AppendLine(customer.FirstName)
                .AppendLine(customer.MidName)
                .AppendLine(customer.LastName ?? "")
                .AppendLine(customer.PhoneNumber ?? "")
                .AppendLine(customer.City.CityName)
                .AppendLine("---");
            //.AppendFormat("Gift wrapping: {0}",
            //    customer.GiftWrap ? "Yes" : "No");

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body.ToString()
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(emailSettings.ServerName, emailSettings.ServerPort, emailSettings.UseSsl);
                    await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {

            }
        }     
    }

    public class EmailSettings
    {
        public string MailFromAddress = "oleg.sergushin11@mail.ru";
        public bool UseSsl = true;
        public string Username = "oleg.sergushin11@mail.ru";
        public string Password = "ir6g2cxADaGv2bNcSRqF";
        public string ServerName = "smtp.mail.ru";
        public int ServerPort = 465;
    }
}
