using MicroserviceCommunication.Catalog.Entities;
using MicroserviceCommunication.Catalog.Enums;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;
using System.Threading;

namespace MicroserviceCommunication.Catalog.Data
{
    public sealed class CatalogContextSeed
    {
        public async Task SeedAsync(CatalogDbContext context, ILogger<CatalogContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(CatalogContextSeed));

            logger.LogInformation("--> Starting Seeding process...");
            await policy.ExecuteAsync(async () =>
            {
                if (await context.Products.AnyAsync() == false)
                {
                    List<ProductBrand> brands;
                    List<ProductType> types;
                    List<Product> products;

                    var colors = Enum.GetValues(typeof(ProductColor)).Cast<ProductColor>().ToList();

                    if (await context.ProductBrands.AnyAsync() == false)
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
                        brands = await context.ProductBrands.ToListAsync();
                    }

                    if (await context.ProductTypes.AnyAsync() == false)
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
                        types = await context.ProductTypes.ToListAsync();
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
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<CatalogContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }

    public static class DbInitializer
    {
        //public static void 
        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", 
                    typeof(TContext).Name);
                
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(new TimeSpan[]
                    {
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(8),
                    });

                //if the sql server container is not created on run docker compose this
                //migration can't fail for network related exception. The retry options for DbContext only 
                //apply to transient exceptions
                // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                retry.Execute(() => InvokeSeeder<TContext>(seeder, context, services));

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", 
                    typeof(TContext).Name);
            }


            return host;
        }

        public static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();

            seeder(context, services);
        }
    }
}
