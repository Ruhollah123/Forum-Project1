using Forum_Project1.Data;
using Forum_Project1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum1_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategoriesByCategory(int categoryId)
        {
            var subCategories = await _context.SubCategories
                .Where(s => s.CategoryId == categoryId)
                .ToListAsync();

            return Ok(subCategories);
        }
    }
}
