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

// for Development purposes
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation($"--> {builder.Configuration.GetConnectionString("Default")}");
try
{
    logger.LogInformation("--> Start migration...");
    await context.Database.MigrateAsync();

    context.Database.BeginTransaction();
    await DbInitializer.Initialize(context);
    context.Database.CommitTransaction();
}
catch (Exception ex)
{
    if (context.Database.CurrentTransaction is not null)
    {
        context.Database.RollbackTransaction();
    }
    logger.LogError(ex, "Error during migration...");
}

await app.RunAsync();
