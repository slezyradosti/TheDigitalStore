using DigitalStore.Models;

namespace DigitalStore.Repos
{
    public interface ICityRepo : IRepo<City>
    {
        List<City> GetRelatedData();
    }
}
