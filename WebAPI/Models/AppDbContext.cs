using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TruckSelling.WebAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Model> Models { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // disable delete cascade
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // unique constraints
            builder.Entity<Truck>()
                .HasIndex(t => t.Chassis)
                .IsUnique();

            builder.Entity<Model>()
                .HasIndex(m => m.Description)
                .IsUnique();

            // relationships
            builder.Entity<Truck>()
              .HasOne(t => t.Model);
        }
    }
}
