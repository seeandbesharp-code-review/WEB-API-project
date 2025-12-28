using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<PageResponseDTO<ProductDTO>>> Get(int position,int skip, [FromQuery] int?[] categoryIds,string? description,int? maxPrice,int? minPrice)
        {
            PageResponseDTO<ProductDTO> pageResponse = await _productService.GetProducts(position,skip,categoryIds, description, maxPrice,minPrice);
            if (pageResponse.Data.Count() > 0)
                return Ok(pageResponse);
            return NoContent();
        }
    }
}
