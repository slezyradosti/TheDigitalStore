using DigitalStore.Models;

namespace DigitalStore.Repos.Interfaces
{
    public interface ICityRepo : IRepo<City>
    {
        List<City> GetRelatedData();
    }
}
