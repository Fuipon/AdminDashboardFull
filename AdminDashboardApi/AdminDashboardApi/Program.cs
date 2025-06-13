using AdminDashboardApi.Data;
using AdminDashboardApi.Models;
using AdminDashboardApi.Seed;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=app.db",
        sqliteOptions => sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
);
builder.Services.AddCors();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Seed();

// CORS 
app.UseCors(policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/auth/login", (LoginRequest req) =>
{
    if (req.Email == "admin@mirra.dev" && req.Password == "admin123")
    {
        return Results.Ok(new { token = "demo" });
    }
    return Results.Unauthorized();
});

app.MapGet("/clients", async (AppDbContext db) =>
{
    var clients = await db.Clients.ToListAsync();
    return Results.Ok(clients);
});

app.MapGet("/payments", async (AppDbContext db, int take = 5) =>
{
    var payments = await db.Payments
        .Include(p => p.Client)
        .OrderByDescending(p => p.CreatedAt)
        .Take(take)
        .Select(p => new {
            p.Id,
            Client = p.Client.Name,
            p.AmountT,
            p.CreatedAt
        })
        .ToListAsync();

    return Results.Ok(payments);
});


app.MapGet("/rate", async (AppDbContext db) =>
{
    var rate = await db.Rates.OrderByDescending(r => r.UpdatedAt).FirstOrDefaultAsync();
    return Results.Ok(rate);
});

app.MapPost("/rate", async (AppDbContext db, [FromBody] decimal rate) =>
{
    var newRate = new Rate
    {
        CurrentRate = rate,
        UpdatedAt = DateTime.UtcNow
    };
    db.Rates.Add(newRate);
    await db.SaveChangesAsync();
    return Results.Ok(new { value = newRate.CurrentRate });
});




//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();

record LoginRequest(string Email, string Password);