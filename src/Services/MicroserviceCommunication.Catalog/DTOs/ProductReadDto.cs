namespace MicroserviceCommunication.Catalog.DTOs
{
    public record ProductReadDto(
        int Id,
        string Title,
        decimal Price,
        string ProductBrand,
        string ProductType,
        string Color
    );
}
