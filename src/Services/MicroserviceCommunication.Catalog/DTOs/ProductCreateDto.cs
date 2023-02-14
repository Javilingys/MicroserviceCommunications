using MicroserviceCommunication.Catalog.Enums;

namespace MicroserviceCommunication.Catalog.DTOs
{
    public record ProductCreateDto(
        string Title,
        string Description,
        decimal Price,
        string ImageUrl,
        ProductColor ProductColor,
        string FiledForCatalogService
    );
}
