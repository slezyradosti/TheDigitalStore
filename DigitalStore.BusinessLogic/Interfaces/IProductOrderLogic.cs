using DigitalStore.Models;
using DigitalStore.Models.NotForDB;

namespace DigitalStore.BusinessLogic.Interfaces
{
    public interface IProductOrderLogic
    {
        public void AddOrderListToDb(Cart cart, Order order);
    }
}