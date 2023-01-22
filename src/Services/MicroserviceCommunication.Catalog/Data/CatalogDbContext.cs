using MicroserviceCommunication.Catalog.Data.Configurations;
using MicroserviceCommunication.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceCommunication.Catalog.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductBrandEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeEntityConfiguration());
        }
    }
}
