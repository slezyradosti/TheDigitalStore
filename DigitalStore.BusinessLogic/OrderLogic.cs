using DigitalStore.BusinessLogic.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;

namespace DigitalStore.BusinessLogic
{
    public class OrderLogic : IOrderLogic
    {
        private readonly IOrderRepo _orderRepo;

        public OrderLogic(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
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
    }
}
