using AppTracker.DataAccess;
using AppTracker.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AppTracker
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppUsageContext>(options =>
                options.UseSqlite("Data Source=apptracker.db"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppUsageContext>();
                dbContext.Database.EnsureCreated();
            }
        }
    }

}
