using DAL.Command.Category;
using DAL.DataContext;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CateogryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CateogryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("Get-All-Categories")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var category = await _context.Categories.Select(x => new Category
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
            }).ToListAsync();
            return Ok(category);    
        }
        [HttpPost("Create-Category")]
        public async Task<IActionResult> PostCategory(CreateCategoryCommand category)
        {
            var newCategory = new Category 
            { 
                CategoryName = category.CategoryName
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return Ok(newCategory);
        }
    }
}
