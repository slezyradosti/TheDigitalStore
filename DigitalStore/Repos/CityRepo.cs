using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class CityRepo : BaseRepo<City>, ICityRepo
    {
        public CityRepo(DigitalStoreContext context) : base(context)
        {
        }

        public List<City> GetRelatedData()
            => Context.Cities.FromSqlInterpolated($"SELECT * FROM City").ToList();
    }
}
