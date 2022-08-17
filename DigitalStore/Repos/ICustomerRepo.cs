using DigitalStore.Models;

namespace DigitalStore.Repos
{
    public interface ICustomerRepo : IRepo<Customer>
    {
        List<Customer> GetRelatedData();
    }
}
