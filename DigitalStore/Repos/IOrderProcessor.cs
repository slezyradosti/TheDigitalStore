namespace DigitalStore.Models.NotForDB
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, Customer customer);
        public Order CreateOrder(Customer customer);
        public void AddOrderListToDb(Cart cart, Order order);
    }
}
