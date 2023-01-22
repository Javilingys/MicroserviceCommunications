using MicroserviceCommunication.Catalog.Entities;
using MicroserviceCommunication.Catalog.Enums;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceCommunication.Catalog.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(CatalogDbContext context, CancellationToken cancellationToken = default)
        {
            if (await context.Products.AnyAsync(cancellationToken) == false)
            {
                List<ProductBrand> brands;
                List<ProductType> types;
                List<Product> products;

                var colors = Enum.GetValues(typeof(ProductColor)).Cast<ProductColor>().ToList();

                if (await context.ProductBrands.AnyAsync(cancellationToken) == false)
                {
                    brands = Enumerable
                        .Range(1, 20)
                        .Select(x =>
                        {
                            return new ProductBrand()
                            {
                                Title = $"Brand {x}"
                            };
                        })
                        .ToList();

                    context.ProductBrands.AddRange(brands);
                    await context.SaveChangesAsync();
                }
                else
                {
                    brands = await context.ProductBrands.ToListAsync(cancellationToken);
                }

                if (await context.ProductTypes.AnyAsync(cancellationToken) == false)
                {
                    types = Enumerable
                        .Range(1, 11)
                        .Select(x =>
                        {
                            return new ProductType()
                            {
                                Title = $"Type {x}"
                            };
                        })
                        .ToList();

                    context.ProductTypes.AddRange(types);
                    await context.SaveChangesAsync();
                }
                else
                {
                    types = await context.ProductTypes.ToListAsync(cancellationToken);
                }

                products = Enumerable.Range(1, 1_000)
                    .Select(x => new Product()
                    {
                        Title = $"Product Number {x}",
                        Description = "Some description xxxxx zzzz zov azov",
                        FiledForCatalogService = "Super secret field",
                        ImageUrl = null,
                        Price = (100 * (1_000 % 9) + Random.Shared.Next(1, 999)),
                        ProductBrandId = brands[Random.Shared.Next(0, brands.Count)].Id,
                        ProductTypeId = types[Random.Shared.Next(0, types.Count)].Id,
                        ProductColor = colors[Random.Shared.Next(0, colors.Count)]
                    })
                    .ToList();

                context.Products.AddRange(products);

                await context.SaveChangesAsync();
            }
        }
    }
}
