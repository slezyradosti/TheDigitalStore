using DigitalStore.Models;

namespace DigitalStore.Repos.Interfaces
{
    public interface ICategoryRepo : IRepo<Category>
    {
        List<Category> Search(string searchString);
    }
}
