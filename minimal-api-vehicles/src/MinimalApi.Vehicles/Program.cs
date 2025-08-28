using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Vehicles.Data;
using MinimalApi.Vehicles.DTOs;
using MinimalApi.Vehicles.Models;
using MinimalApi.Vehicles.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurações
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinimalApi.Vehicles", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira 'Bearer' [espaço] e o token JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { }
        }
    });
});

// DbContext MySQL (Pomelo)
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 31))));

// Services
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditorOrAdmin", policy => policy.RequireRole("Editor", "Admin"));
});

var app = builder.Build();

// Migrate + Seed (apenas em dev/first-run — em produção, preferível rodar migrations separadamente)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await db.EnsureSeedDataAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Home
app.MapGet("/", () => Results.Json(new { App = "MinimalApi.Vehicles", Version = "1.0" }))
   .AllowAnonymous()
   .WithName("Home");

// Login
app.MapPost("/login", async (LoginRequest login, AppDbContext db, IAuthService auth) =>
{
    var user = await db.AdminUsers.SingleOrDefaultAsync(u => u.Email == login.Email);
    if (user == null) return Results.Unauthorized();
    if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash)) return Results.Unauthorized();
    var token = auth.GenerateToken(user);
    return Results.Ok(new { token });
}).Accepts<LoginRequest>("application/json");

// Vehicles
app.MapPost("/vehicles", async (VehicleDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin) || dto.Vin.Length > 17) return Results.BadRequest(new { error = "VIN inválido" });

    var vehicle = new Vehicle
    {
        Make = dto.Make,
        Model = dto.Model,
        Vin = dto.Vin,
        Year = dto.Year
    };

    db.Vehicles.Add(vehicle);
    await db.SaveChangesAsync();
    return Results.Created($"/vehicles/{vehicle.Id}", vehicle);
}).RequireAuthorization("EditorOrAdmin");

app.MapGet("/vehicles", async (AppDbContext db) => await db.Vehicles.ToListAsync())
   .RequireAuthorization()
   .WithName("GetVehicles");

app.MapGet("/vehicles/{id:int}", async (int id, AppDbContext db) =>
{
    var v = await db.Vehicles.FindAsync(id);
    return v is null ? Results.NotFound() : Results.Ok(v);
}).RequireAuthorization();

app.MapPut("/vehicles/{id:int}", async (int id, VehicleDto dto, AppDbContext db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);
    if (vehicle == null) return Results.NotFound();

    vehicle.Make = dto.Make;
    vehicle.Model = dto.Model;
    vehicle.Vin = dto.Vin;
    vehicle.Year = dto.Year;

    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization("AdminOnly");

app.MapDelete("/vehicles/{id:int}", async (int id, AppDbContext db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);
    if (vehicle == null) return Results.NotFound();
    db.Vehicles.Remove(vehicle);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization("AdminOnly");

// Admins management
app.MapPost("/admins", async (AdminCreateDto dto, AppDbContext db) =>
{
    if (await db.AdminUsers.AnyAsync(a => a.Email == dto.Email)) return Results.Conflict(new { error = "Email já cadastrado" });

    var admin = new AdminUser
    {
        Email = dto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Role = dto.Role
    };

    db.AdminUsers.Add(admin);
    await db.SaveChangesAsync();
    return Results.Created($"/admins/{admin.Id}", new { admin.Id, admin.Email, admin.Role });
}).RequireAuthorization("AdminOnly");

app.MapGet("/admins", async (AppDbContext db) => await db.AdminUsers.Select(a => new { a.Id, a.Email, a.Role }).ToListAsync())
   .RequireAuthorization("AdminOnly");

app.Run();
