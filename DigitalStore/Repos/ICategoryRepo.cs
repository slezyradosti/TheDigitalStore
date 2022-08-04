using DigitalStore.Models;

namespace DigitalStore.Repos
{
    public interface ICategoryRepo : IRepo<Category>
    {
        List<Category> Search(string searchString);
    }
}
