using System;

using Microsoft.EntityFrameworkCore;

using NLSL.SKS.Package.DataAccess.Entities;

namespace NLSL.SKS.Package.DataAccess.Sql
{
    public sealed class PackageContext : DbContext
    {
        public DbSet<Parcel> Parcels
        {
            get;
            set;
        }

        public DbSet<Warehouse> Warehouses
        {
            get;
            set;
        }

        public PackageContext(DbContextOptions<PackageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}