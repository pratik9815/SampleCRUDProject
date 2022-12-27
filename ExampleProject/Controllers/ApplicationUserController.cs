using DAL.Command.ApplicationUser;
using DAL.DataContext;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
      

        public ApplicationUserController(ApplicationDbContext context)
        {
            _context = context;
           
        }
    
        [HttpGet("Get-User")]
        public async Task<ActionResult<List<CreateUserCommand>>> GetUsers()
        {
            var users = await _context
                                .Users
                                .Select(x => new CreateUserCommand
                                {
                                    UserName = x.UserName,
                                    Email = x.Email,
                                    PhoneNumber=x.PhoneNumber,  
                                }).ToListAsync();
            return(users);  
        }
        [HttpPost("Create-user")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserCommand user)
        {   
            var newUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
           
            return Ok(newUser);
        }
    }
}
