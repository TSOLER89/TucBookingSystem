using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TucBookingSystem.Api.Data;
using TucBookingSystem.Api.Middleware;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Skriv: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("http://localhost:5045",
        policy.WithOrigins(
                "https://localhost:5045",
                "https://localhost:7116",
                "http://localhost:5000",
                "https://localhost:5001",
                "http://localhost:7214")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
bool IsIdentityPasswordHash(string passwordHash) =>
    passwordHash.StartsWith("AQAAAA", StringComparison.Ordinal);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var passwordHasher = new PasswordHasher<User>();
    var adminEmail = builder.Configuration["Admin:Email"] ?? "admin@admin.se";
    var adminFullName = builder.Configuration["Admin:FullName"] ?? "Admin";
    var adminPassword = builder.Configuration["Admin:Password"];

    dbContext.Database.EnsureCreated();

    var adminUser = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == adminEmail);

    if (adminUser is null)
    {
        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogWarning("Skipping admin seed because Admin:Password is not configured.");
        }
        else
        {
            var newAdmin = new User
            {
                FullName = adminFullName,
                Email = adminEmail,
                Role = "Admin"
            };

            newAdmin.PasswordHash = passwordHasher.HashPassword(newAdmin, adminPassword);
            dbContext.Users.Add(newAdmin);
            logger.LogInformation("Seeded admin user {Email}.", adminEmail);
        }
    }
    else
    {
        var hasChanges = false;

        if (!string.Equals(adminUser.FullName, adminFullName, StringComparison.Ordinal))
        {
            adminUser.FullName = adminFullName;
            hasChanges = true;
        }

        if (!string.Equals(adminUser.Role, "Admin", StringComparison.Ordinal))
        {
            adminUser.Role = "Admin";
            hasChanges = true;
        }

        if (!IsIdentityPasswordHash(adminUser.PasswordHash))
        {
            var passwordToHash = string.IsNullOrWhiteSpace(adminPassword)
                ? adminUser.PasswordHash
                : adminPassword;

            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, passwordToHash);
            hasChanges = true;
            logger.LogInformation("Migrated admin password for {Email} to hashed storage.", adminEmail);
        }

        if (hasChanges)
        {
            await dbContext.SaveChangesAsync();
        }
    }

    if (adminUser is null && !string.IsNullOrWhiteSpace(adminPassword))
    {
        await dbContext.SaveChangesAsync();
    }
}

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazor");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program class accessible to integration tests
public partial class Program { }
