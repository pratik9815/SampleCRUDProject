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
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products = await _context.Products.Select(x => new Product
            {
                Name =  x.Name,
                Description = x.Description,
                Price = x.Price,
                CreatedDate = x.CreatedDate,
                

            }).ToListAsync();
            return(products);   
        }
        [HttpPost("Create-Product")]
        public async Task<ActionResult<string>> CreateProduct(CreateProductCommand product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description= product.Description,
                CreatedDate = product.CreatedDate,  
                Price = product.Price,
                CategoryId = product.CategoryId,
            };
             _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();  
            return Ok("The product has been created");
        }

    }
}
