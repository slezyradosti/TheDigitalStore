using System.Collections.Generic;
using DigitalStore.Models;

namespace DigitalStore.Repos
{
    public interface IProductRepo : IRepo<Product>
    {
        List<Product> Search(string searchString);
       // List<Product> GetAllPromotionalProducts();
        List<Product> GetRelatedData();
    }
}
