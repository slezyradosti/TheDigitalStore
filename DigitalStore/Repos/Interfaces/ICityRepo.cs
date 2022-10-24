using DigitalStore.Models;
using DigitalStore.Models.NotForDB;

namespace DigitalStore.Repos.Interfaces
{
    public interface ICityRepo : IRepo<City>
    {
        List<City> GetRelatedData();
    }
}
