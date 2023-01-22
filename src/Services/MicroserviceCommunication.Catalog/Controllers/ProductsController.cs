using AutoMapper;
using AutoMapper.QueryableExtensions;
using MicroserviceCommunication.Catalog.Data;
using MicroserviceCommunication.Catalog.DTOs;
using MicroserviceCommunication.Catalog.Entities;
using MicroserviceCommunication.Catalog.MapperProfiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceCommunication.Catalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CatalogDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(CatalogDbContext context, IMapper mapper, ILogger<ProductsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            _logger.LogInformation("--> Get Products from Catalog Service");

            var products = await _context.Products
                .Include(x => x.ProductBrand)
                .Include(x => x.ProductType)
                .Take(50)
                .ProjectTo<ProductReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(products);
        }
    }
}
