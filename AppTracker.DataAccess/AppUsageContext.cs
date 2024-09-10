using AppTracker.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker.DataAccess
{
    public class AppUsageContext : DbContext
    {
        public DbSet<AppUsage> AppUsages { get; set; }

        public AppUsageContext() { }

        public AppUsageContext(DbContextOptions<AppUsageContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=apptracker.db");
            }
        }
    }
}
