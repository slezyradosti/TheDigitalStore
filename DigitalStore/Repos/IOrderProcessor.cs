namespace DigitalStore.Models.NotForDB
{
    public interface IOrderProcessor
    {
        public Task SendEmailAsync(string email, string subject, string message);
        public Order CreateOrder(Customer customer);
        public void AddOrderListToDb(Cart cart, Order order);
        public Task SendPurchaseEmailAsync(Customer customer, string subject, Cart cart);
    }
}
