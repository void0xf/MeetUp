using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("connect/token/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true // Set to true or handle email confirmation as needed
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Add any additional claims if needed
                var claims = new[]
                {
                    new Claim(JwtClaimTypes.Name, model.Username),
                    new Claim(JwtClaimTypes.Email, model.Email)
                };

                var claimResult = await _userManager.AddClaimsAsync(user, claims);
                if (!claimResult.Succeeded)
                {
                    return BadRequest(claimResult.Errors);
                }

                return Ok(
                    new { Message = "User registered successfully", Description = "Success" }
                );
            }

            return BadRequest(result.Errors);
        }
    }
}
