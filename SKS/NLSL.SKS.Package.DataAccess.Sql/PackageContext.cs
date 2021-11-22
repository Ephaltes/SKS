using System;
using System.Diagnostics.CodeAnalysis;

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
        
        public PackageContext(DbContextOptions<PackageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //placeholder
            base.OnModelCreating(modelBuilder);
        }
    }
}