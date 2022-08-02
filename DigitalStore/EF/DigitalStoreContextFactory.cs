using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DigitalStore.EF
{
    public class DigitalStoreContextFactory : IDesignTimeDbContextFactory<DigitalStoreContext>
    {
        public DigitalStoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DigitalStoreContext>();
            var connectionString = @"server=DESKTOP-QT7HQTJ;database=DigitalStore;integrated security=True;
                    MultipleActiveResultSets=True;App=EntityFramework;";
            optionsBuilder.UseSqlServer(
                    connectionString,
                    options => options.EnableRetryOnFailure())
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning));
            return new DigitalStoreContext(optionsBuilder.Options);
        }
    }
}
