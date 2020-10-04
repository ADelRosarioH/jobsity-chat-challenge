using System.Threading.Tasks;
using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobsityStocksChat.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenClaimService _tokenClaimService;

        public AuthController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ITokenClaimService tokenClaimService, 
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenClaimService = tokenClaimService;
            _logger = logger;

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] NewUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "User registration failed." });
            }

            var newUser = new ApplicationUser() { UserName = model.UserName, Email = model.Email };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "User registration failed.", result.Errors });
            }

            return Ok(new { Message = "New user was registered." });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { 
                    Message = "Missing UserName or Password." 
                });
            }

            var signInResult = await _signInManager.PasswordSignInAsync(credentials.UserName, credentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Unauthorized(new { 
                    Message = "Invalid UserName or Password."
                });
            }

            var token = _tokenClaimService.GetToken(credentials.UserName);

            return Ok(new { Token = token });
        }
    }
}
