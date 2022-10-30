using DigitalStore.Models;

namespace DigitalStore.Repos.Interfaces
{
    public interface IProductRepo : IRepo<Product>
    {
        List<Product> Search(string searchString);
        List<Product> Search(int? categoryId);
        List<Product> Search(Category category);
        List<Product> GetRelatedData();
        List<Product> GetTenRandomItems(int productCount);
    }
}
