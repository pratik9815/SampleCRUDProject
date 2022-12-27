using DAL.Command.ApplicationUser;
using DAL.DataContext;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ApplicationUserController(ApplicationDbContext context ,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
    
    
        [HttpPost("Create-user")]
        public async Task<ActionResult<string>> CreateUser([FromBody]CreateUserCommand user)
        {   
            var newUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(newUser.UserName);
        }
        [HttpPost("User-SignIn")]
        public async Task<ActionResult> AuthenticateUser([FromBody] AuthenticateRequest request)
        {
            var identityUser = await _userManager.FindByNameAsync(request.UserName);    
            if (identityUser == null)
                return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized();
            return Ok(result.Succeeded);
        }
    }
}
