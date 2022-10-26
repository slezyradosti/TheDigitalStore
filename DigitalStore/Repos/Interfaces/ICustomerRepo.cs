using DigitalStore.Models;

namespace DigitalStore.Repos.Interfaces
{
    public interface ICustomerRepo : IRepo<Customer>
    {
        List<Customer> GetRelatedData();
        public List<Customer> GetCustomerIdByUserId(string Id);
    }
}
