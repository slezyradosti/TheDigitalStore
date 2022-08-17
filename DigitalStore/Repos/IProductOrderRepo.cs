using DigitalStore.Models;

namespace DigitalStore.Repos
{
    public interface IProductOrderRepo : IRepo<ProductOrder>
    {
        List<ProductOrder> GetRelatedData();
    }
}
