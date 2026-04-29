using DTOs;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Services;
using System.Text.Json;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private const string _productsVersionKey = "products_version";

        private readonly IProductService _productService;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            IDistributedCache cache,
            IConfiguration configuration,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<PageResponseDTO<ProductDTO>>> Get([FromQuery] int?[] categoryIds,string? description,int? maxPrice,int? minPrice,int position = 1,int skip = 8)
        {
            int version = await GetCacheVersion();

            string categoryIdsStr = categoryIds != null
                ? string.Join(",", categoryIds.Where(c => c.HasValue).Select(c => c.Value))
                : "";

            string cacheKey =
                $"products_v{version}_{categoryIdsStr}_{description ?? ""}_{maxPrice ?? 0}_{minPrice ?? 0}_{position}_{skip}";

            var cached = await TryGetFromCache<PageResponseDTO<ProductDTO>>(cacheKey);
            if (cached != null) return Ok(cached);

            var pageResponse = await _productService.GetProducts(
                position, skip, categoryIds, description, maxPrice, minPrice);

            if (!pageResponse.Data.Any()) return NoContent();

            await TrySetCache(cacheKey, pageResponse);
            return Ok(pageResponse);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            int version = await GetCacheVersion();

            string cacheKey = $"product_v{version}_{id}";

            var cached = await TryGetFromCache<ProductDTO>(cacheKey);
            if (cached != null) return Ok(cached);

            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();

            await TrySetCache(cacheKey, product);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post([FromBody] PostProductDTO newProduct)
        {
            var returnedProduct = await _productService.AddProduct(newProduct);
            if (returnedProduct == null) return BadRequest();

            await InvalidateProductCache();

            return CreatedAtAction(nameof(Get), new { id = returnedProduct.Id }, returnedProduct);
        }


        private async Task<T?> TryGetFromCache<T>(string key) where T : class
        {
            try
            {
                var json = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                    return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache GET failed for key {Key}", key);
            }
            return null;
        }

        private async Task TrySetCache<T>(string key, T value)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                int ttl = _configuration.GetValue<int>("CacheSettings:ProductCacheTTLMinutes");

                await _cache.SetStringAsync(key, json,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ttl)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache SET failed for key {Key}", key);
            }
        }


        private async Task<int> GetCacheVersion()
        {
            try
            {
                var versionStr = await _cache.GetStringAsync(_productsVersionKey);
                return string.IsNullOrEmpty(versionStr) ? 1 : int.Parse(versionStr);
            }
            catch
            {
                return 1;
            }
        }

        private async Task InvalidateProductCache()
        {
            try
            {
                var versionStr = await _cache.GetStringAsync(_productsVersionKey);
                int version = string.IsNullOrEmpty(versionStr) ? 1 : int.Parse(versionStr);

                await _cache.SetStringAsync(_productsVersionKey, (version + 1).ToString());

                _logger.LogInformation("Product cache invalidated via versioning");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache invalidation failed");
            }
        }
    }
}