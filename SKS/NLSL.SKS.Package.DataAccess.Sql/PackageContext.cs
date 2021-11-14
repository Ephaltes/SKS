using System;

using Microsoft.EntityFrameworkCore;

using NLSL.SKS.Package.DataAccess.Entities;

namespace NLSL.SKS.Package.DataAccess.Sql
{
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
        
        public PackageContext(DbContextOptions<PackageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //Placeholder for future implementation
        }
    }
}