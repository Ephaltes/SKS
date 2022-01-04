using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using NLSL.SKS.Package.DataAccess.Entities;

namespace NLSL.SKS.Package.DataAccess.Sql
{
    [ExcludeFromCodeCoverage]
    public class PackageContext : DbContext
    {
        public virtual DbSet<Parcel> Parcels
        {
            get;
            set;
        }

        public virtual DbSet<Warehouse> Warehouses
        {
            get;
            set;
        }
        
        public virtual DbSet<Truck> Trucks 
        {
            get;
            set;
        }
        
        public virtual DbSet<Transferwarehouse> Transferwarehouses
        {
            get;
            set;
        }
        
        public virtual DbSet<WarehouseNextHops> WarehouseNextHops
        {
            get;
            set;
        }

        public virtual DbSet<WebHook> WebHooks
        {
            get;
            set;
        }
        public PackageContext(DbContextOptions<PackageContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //placeholder
          base.OnModelCreating(modelBuilder);

          modelBuilder.Entity<Parcel>()
              .HasOne(r => r.Recipient)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);
          
          modelBuilder.Entity<Parcel>()
              .HasOne(r => r.Sender)
              .WithMany()
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}