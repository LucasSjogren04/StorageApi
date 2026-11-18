using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.DTOs;
using StorageApi.Models;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(StorageApiContext context) : ControllerBase
    {
        // GET: api/products
        // GET: api/products?category=verktyg
        // GET: api/products?name=hammare
        // GET: api/products?category=verktyg&name=hammare
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
            [FromQuery] string? category,
            [FromQuery] string? name)
        {
            var query = context.Product.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category))
            {
                var categoryTerm = category.Trim();
                query = query.Where(p => p.Category.Contains(categoryTerm));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var nameTerm = name.Trim();
                query = query.Where(p => p.Name.Contains(nameTerm));
            }

            var products = await query
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Count = p.Count
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/stats
        [HttpGet("stats")]
        public async Task<ActionResult<ProductStatsDto>> GetStats()
        {
            var stats = await context.Product
                .GroupBy(_ => 1)
                .Select(g => new ProductStatsDto
                {
                    TotalProducts = g.Sum(p => (long)p.Count),
                    TotalInventoryValue = g.Sum(p => (long)p.Price * p.Count),
                    AveragePrice = Math.Round(g.Average(p => (double)p.Price), 2)
                })
                .FirstOrDefaultAsync();

            return Ok(stats ?? new ProductStatsDto());
        }

        // GET: api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await context.Product
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Count = p.Count
                })
                .FirstOrDefaultAsync();

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = dto.Category,
                Shelf = dto.Shelf,
                Count = dto.Count,
                Description = dto.Description
            };

            context.Product.Add(product);
            await context.SaveChangesAsync();

            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Count = product.Count
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, result);
        }
    }
}
