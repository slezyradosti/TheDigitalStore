using DigitalStore.Models;

namespace DigitalStore.BusinessLogic.Interfaces
{
    public interface IOrderLogic
    {
        public Order CreateOrder(Customer customer);
    }
}