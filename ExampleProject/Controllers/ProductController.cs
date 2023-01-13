using DAL.Command.Product;
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
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("Get-Product")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                CreatedDate = x.CreatedDate,
            }).ToListAsync();
            return (products);
        }
        [HttpGet("Search-Item/{name}")]
         public async Task<ActionResult<CreateProductCommand>> SearchItem(string name)
        {
            var items = await _context.Products.Where(s =>
             s.Name.Contains(name)).ToListAsync();
            return Ok(items);
        }

        [HttpPost("Create-Product")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                CreatedDate = product.CreatedDate,
                Price = product.Price,
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("Update-Product/{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute]Guid id, [FromBody]Product product)
        {
           var getProduct = await _context.Products.FindAsync(id);
            if (getProduct == null)
                return BadRequest();
            getProduct.Name = product.Name;
            getProduct.Description = product.Description;
            getProduct.Price = product.Price;
            await _context.SaveChangesAsync();
            return Ok(getProduct);

        }

        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute]Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product ==null) return BadRequest();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
    }
}
