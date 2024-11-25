using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Models;
using System.Threading.Tasks;
using EcommerceLib.Dtos;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using EcommerceAPI.Services;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _userService = userService;
        }
        [HttpPost("register")]
        //[Authorize]
        public async Task<IActionResult> Register(UserCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // create user
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(new { Error = "Register failed", Details = result.Errors });
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                // Kiểm tra xem Role có tồn tại hay không
                var roleExists = await _roleManager.RoleExistsAsync(model.Role);
                if (!roleExists)
                {
                    // Nếu không tồn tại role, rollback việc tạo người dùng
                    await _userManager.DeleteAsync(user);
                    return BadRequest(new { Error = $"Role '{model.Role}' does not exist" });
                }

                // Thêm User vào Role nếu Role tồn tại
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!roleResult.Succeeded)
                {
                    // Nếu không thêm được role, rollback việc tạo người dùng
                    await _userManager.DeleteAsync(user);
                    return BadRequest(new { Error = "Failed to assign role", Details = roleResult.Errors });
                }
            }

            // Lấy danh sách Roles của User
            var roles = await _userManager.GetRolesAsync(user);

            // Trả về thông tin người dùng cùng với các role
            var userInfo = new
            {
                user.UserName,
                user.Email,
                user.PhoneNumber,
                Roles = roles, // Thêm các role của người dùng vào response
            };

            return Ok(new { Message = "User registered successfully", User = userInfo });
        }

        [HttpGet("basic-users")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersByRoleAsync("Basic User");
            return Ok(users);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest(new { Error = "Invalid username or password" });
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var token = GenerateJwtToken(model.Username); 

                // Return token & roles
                return Ok(new { token, roles });
            }

            return BadRequest(new { Error = "Invalid username or password" });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful" });
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, username) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
