using DigitalStore.Models;

namespace DigitalStore.Repos.Interfaces
{
    public interface IOrderRepo : IRepo<Order>
    {
        List<Order> GetRelatedData();
        public List<Order> GetCustomerOrdersList(int customerId);
    }
}
