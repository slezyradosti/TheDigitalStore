using DigitalStore.Models;
using DigitalStore.Models.NotForDB;

namespace DigitalStore.BusinessLogic.Interfaces
{
    public interface IOrderProcessor
    {
        public Task SendEmailAsync(string email, string subject, string message);
        public Task SendPurchaseEmailAsync(Customer customer, string subject, Cart cart);
    }
}
