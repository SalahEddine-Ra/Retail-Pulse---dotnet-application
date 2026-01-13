using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RetailPulse.Data;
using RetailPulse.Services;
using RetailPulse.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


// --------------------------------------------------
// Application Builder Initialization
// --------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Database Configuration
// --------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("RetailPulseDbConnection")
    ?? throw new InvalidCastException("Connection string 'RetailPulseDbConnection' not found.");
builder.Services.AddDbContext<RetailPulseDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// --------------------------------------------------
// Service Registrations
// --------------------------------------------------
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// --------------------------------------------------
// Authentication Configuration
// --------------------------------------------------
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
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    ClockSkew = TimeSpan.Zero 
    };
});

// --------------------------------------------------
// API & Swagger Configuration
// --------------------------------------------------
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --------------------------------------------------
// Application Pipeline Configuration
// --------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapControllers();
app.Run();