using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AdminDashboardApi.Data;

namespace AdminDashboardApi.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=admin_dashboard;Username=postgres;Password=postgres");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
