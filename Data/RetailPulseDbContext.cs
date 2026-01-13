using System;
using Microsoft.EntityFrameworkCore;
using RetailPulse.Models;

namespace RetailPulse.Data
{
    public class RetailPulseDbContext : DbContext
    {
        public RetailPulseDbContext(DbContextOptions<RetailPulseDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products {get; set;}
        public DbSet<Sale> Sales { get; set; }
        public DbSet<User> Users { get; set; }
    }
}