using DigitalStore.Models;

namespace DigitalStore.Repos.Interfaces
{
    public interface IProductOrderRepo : IRepo<ProductOrder>
    {
        List<ProductOrder> GetRelatedData();
    }
}
