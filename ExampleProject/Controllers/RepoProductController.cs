using DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepoProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public RepoProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("Get_Product")]
        public ActionResult GetProduct()
        {
            var product = _productRepository.GetProducts();

            if(!ModelState.IsValid) return BadRequest();
                return Ok(product);
        }
    }
}
