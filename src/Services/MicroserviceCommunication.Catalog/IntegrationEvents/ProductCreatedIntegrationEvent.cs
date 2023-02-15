namespace MicroserviceCommunication.Catalog.IntegrationEvents
{
    public sealed record ProductCreatedIntegrationEvent
    {
        public Guid CommandId { get; init; }

        public DateTime Timestamp { get; init; }

        public int ProductId { get; init; }

        public string Title { get; init; }

        public string Description { get; init; }

        public decimal Price { get; init; }

        public int ProductBrandId { get; init; }

        public int ProductTypeId { get; init; }
    }
}
