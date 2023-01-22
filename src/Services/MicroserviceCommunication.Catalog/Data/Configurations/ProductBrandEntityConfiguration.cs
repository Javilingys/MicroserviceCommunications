using MicroserviceCommunication.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceCommunication.Catalog.Data.Configurations
{
    public class ProductBrandEntityConfiguration : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
