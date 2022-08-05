using DigitalStore.Models;

namespace DigitalStore.Repos
{
    public interface IProductRepo : IRepo<Product>
    {
        List<Product> Search(string searchString);
        List<Product> Search(int? categoryId);
        List<Product> Search(Category category);
        // List<Product> GetAllPromotionalProducts();
        List<Product> GetRelatedData();
    }
}
