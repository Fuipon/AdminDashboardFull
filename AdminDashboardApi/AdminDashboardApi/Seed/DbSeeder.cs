using AdminDashboardApi.Data;
using AdminDashboardApi.Models;

namespace AdminDashboardApi.Seed
{
    public static class DbSeeder
    {
        public static void Seed(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (db.Clients.Any()) return;

            var clients = new[]
            {
                new Client { Name = "Алиса", Email = "alice@example.com", BalanceT = 100 },
                new Client { Name = "Женя", Email = "evghenii@example.com", BalanceT = 200 },
                new Client { Name = "Алексей", Email = "alexey@example.com", BalanceT = 150 },
            };

            db.Clients.AddRange(clients);
            db.SaveChanges();

            var payments = new[]
            {
                new Payment { ClientId = clients[0].Id, AmountT = 50, CreatedAt = DateTime.UtcNow.AddMinutes(-10) },
                new Payment { ClientId = clients[1].Id, AmountT = 75, CreatedAt = DateTime.UtcNow.AddMinutes(-20) },
                new Payment { ClientId = clients[2].Id, AmountT = 30, CreatedAt = DateTime.UtcNow.AddMinutes(-30) },
                new Payment { ClientId = clients[0].Id, AmountT = 20, CreatedAt = DateTime.UtcNow.AddMinutes(-40) },
                new Payment { ClientId = clients[1].Id, AmountT = 25, CreatedAt = DateTime.UtcNow.AddMinutes(-50) },
            };

            db.Payments.AddRange(payments);

            db.Rates.Add(new Rate { CurrentRate = 10, UpdatedAt = DateTime.UtcNow });

            db.SaveChanges();
        }
    }
}
