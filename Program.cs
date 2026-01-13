using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RetailPulse.Data;
using RetailPulse.Services;
using RetailPulse.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("RetailPulseDbConnection")
        ?? throw new InvalidCastException("Connection string 'RetailPulseDbConnection' not found.");
builder.Services.AddDbContext<RetailPulseDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // This creates the UI page
}

app.MapControllers();
app.Run();
