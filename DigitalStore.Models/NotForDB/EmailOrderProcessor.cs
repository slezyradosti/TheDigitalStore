using System.Net;
using System.Net.Mail;
using System.Text;


namespace DigitalStore.Models.NotForDB
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials =
                    new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

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
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.AddressLine1)
                    .AppendLine(shippingInfo.AddressLine2 ?? "")
                    .AppendLine(shippingInfo.AddressLine3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine("---")
                    .AppendFormat("Gift wrapping: {0}",
                        shippingInfo.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,	// From
                                       emailSettings.MailToAddress,		// To
                                       "The new order has been sent!",		// Thepe
                                       body.ToString()); 				// Body of letter

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }

    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "digitaltore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"c:\digital_store_emails";
    }
}
