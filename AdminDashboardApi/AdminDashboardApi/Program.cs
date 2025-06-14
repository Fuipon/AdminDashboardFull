using AdminDashboardApi.Auth;
using AdminDashboardApi.Data;
using AdminDashboardApi.Models;
using AdminDashboardApi.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// JWT settings
var jwtSettings = new JwtSettings
{
    SecretKey = "nL3g4@u!Q#2v7ZkXsD9w1HtYpG$e0RcMj",
    Issuer = "AdminDashboard",
    Audience = "AdminDashboardClient",
    ExpirationMinutes = 60
};

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

//

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// CORS 
app.UseCors(policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/auth/login", (LoginRequest req, TokenService tokenService) =>
{
    if (req.Email == "admin@mirra.dev" && req.Password == "admin123")
    {
        var token = tokenService.GenerateToken(req.Email);
        return Results.Ok(new { token });
    }
    return Results.Unauthorized();
});

app.MapGet("/clients", async (AppDbContext db) =>
{
    var clients = await db.Clients.ToListAsync();
    return Results.Ok(clients);
})
.RequireAuthorization();

// Получить клиента по id
app.MapGet("/clients/{id}", async (int id, AppDbContext db) =>
{
    var client = await db.Clients.FindAsync(id);
    return client is not null ? Results.Ok(client) : Results.NotFound();
}).RequireAuthorization();

// Создать клиента
app.MapPost("/clients", async (Client client, AppDbContext db) =>
{
    db.Clients.Add(client);
    await db.SaveChangesAsync();
    return Results.Created($"/clients/{client.Id}", client);
}).RequireAuthorization();

// Обновить клиента
app.MapPut("/clients/{id}", async (int id, Client inputClient, AppDbContext db) =>
{
    var client = await db.Clients.FindAsync(id);
    if (client is null) return Results.NotFound();

    client.Name = inputClient.Name;
    client.Email = inputClient.Email;
    client.BalanceT = inputClient.BalanceT;
    client.Tags = inputClient.Tags;

    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

// Удалить клиента
app.MapDelete("/clients/{id}", async (int id, AppDbContext db) =>
{
    var client = await db.Clients.FindAsync(id);
    if (client is null) return Results.NotFound();

    db.Clients.Remove(client);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

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
})
.RequireAuthorization();


app.MapGet("/rate", async (AppDbContext db) =>
{
    var rate = await db.Rates.OrderByDescending(r => r.UpdatedAt).FirstOrDefaultAsync();
    return Results.Ok(rate);
})
.RequireAuthorization();

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
})
.RequireAuthorization();




//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    DbSeeder.Seed(app);
}



app.Run();

public record LoginRequest(string Email, string Password);