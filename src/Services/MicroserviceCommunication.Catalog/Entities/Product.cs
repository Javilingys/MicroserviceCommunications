using MicroserviceCommunication.Catalog.Enums;

namespace MicroserviceCommunication.Catalog.Entities
{
    public sealed class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }


        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public ProductColor ProductColor { get; set; }

        /// <summary>
        /// Simulate field for only catalog service. This field should not transfer to outside
        /// </summary>
        public string FiledForCatalogService { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        // --------- NAVIGATIONAL PROPERTIES --------------

        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
