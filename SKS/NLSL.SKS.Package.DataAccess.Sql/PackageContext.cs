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

        public PackageContext(DbContextOptions<PackageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}