using DigitalStore.Repos;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace DigitalStore.Models.NotForDB
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductOrderRepo _productOrderRepo;

        public EmailOrderProcessor(IOrderRepo _orepo, IProductOrderRepo productOrderRepo, EmailSettings settings)
        {
            _orderRepo = _orepo;
            _productOrderRepo = productOrderRepo;
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, Customer customer)
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
                    .AppendLine(customer.FirstName)
                    .AppendLine(customer.MidName)
                    .AppendLine(customer.LastName ?? "")
                    .AppendLine(customer.PhoneNumber ?? "")
                    .AppendLine(customer.City.CityName)
                    //.AppendLine(customer.Country)
                    .AppendLine("---");
                    //.AppendFormat("Gift wrapping: {0}",
                    //    customer.GiftWrap ? "Yes" : "No");

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

        public Order CreateOrder(Customer customer)
        {
            Order newOrder = new Order();
            newOrder.OrderDate = DateTime.Now;
            newOrder.DeliveryDate = DateTime.Now;
            newOrder.CityId = customer.CityId;
            newOrder.CustomerId = customer.Id;

            _orderRepo.Add(newOrder);

            return newOrder;
        }

        public void AddOrderListToDb(Cart cart, Order order)
        {
            foreach (var line in cart.Lines)
            {
                ProductOrder newProductOrder = new ProductOrder();
                newProductOrder.ProductId = line.Product.Id;
                newProductOrder.OrderId = order.Id;

                _productOrderRepo.Add(newProductOrder);
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
