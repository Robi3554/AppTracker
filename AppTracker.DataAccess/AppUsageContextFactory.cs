using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace AppTracker.DataAccess
{
    public class AppUsageContextFactory : IDesignTimeDbContextFactory<AppUsageContext>
    {
        public AppUsageContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppUsageContext>();
            optionsBuilder.UseSqlite("Data Source=apptracker.db");

            return new AppUsageContext(optionsBuilder.Options);
        }
    }
}
