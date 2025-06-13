using AdminDashboardApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboardApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Rate> Rates => Set<Rate>();
    }
}
