using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            IEnumerable<CategoryDTO> categories = await _categoryService.GetCategories();
            if (categories.Count() > 0)
                return Ok(categories);
            return NoContent();
        }
    }
}
