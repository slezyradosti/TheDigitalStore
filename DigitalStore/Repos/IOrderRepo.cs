using DigitalStore.Models;

namespace DigitalStore.Repos
{
    public interface IOrderRepo : IRepo<Order>
    {
        List<Order> GetRelatedData();
    }
}
