using MicroserviceCommunication.Catalog.Data;
using MicroserviceCommunication.Catalog.MapperProfiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<CatalogDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    logger.LogInformation("--> Applying migrations in Catalog Service");

    app.MigrateDbContext<CatalogDbContext>((context, services) =>
    {
        var logger = services.GetService<ILogger<CatalogContextSeed>>();

        new CatalogContextSeed().SeedAsync(context, logger).Wait();
    });
}
catch (Exception ex)
{
    logger.LogError(ex, "Error during migration...");
}

await app.RunAsync();
