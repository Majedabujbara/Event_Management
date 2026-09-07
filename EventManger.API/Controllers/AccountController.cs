using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Identity;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;
        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signManger, RoleManager<ApplicationRole> roleManager,IJwtService jwtService)
        {
            _userManager = userManager;

            _signManger = signManger;

            _roleManager = roleManager;

            _jwtService = jwtService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
        {
            //validation
            if(!ModelState.IsValid)
            {
                var errors =string.Join(" | ",ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errors);
            }

            //user might have missing fields (check later)
            var user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PersonName = registerDTO.PersonName,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if(result.Succeeded)
            {
                //sign in 
                await _signManger.SignInAsync(user,isPersistent:false);

                var response =_jwtService.CreateJwtToken(user);

                return Ok(response);
            }
            else
            {
                if(result.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    return Problem("Email is already in use");
                }
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Problem(errors);
            }
        }

        [HttpGet("IsEmailAlreadyRegistered")]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(user == null);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApplicationUser>> PostLogin(LoginDTO loginDTO)
        {
            //validation
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errors);
            }

            var result = await _signManger.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure:false);
            if(result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email); 
                if(user == null)
                {
                    return NoContent(); 
                }

                var response = _jwtService.CreateJwtToken(user);
                return Ok(response);
            }
            else
            {
                return Problem("wrong password or email");
            }
        }
        [HttpGet("Logout")]
        public async Task<IActionResult> GetLogout()
        {
            await _signManger.SignOutAsync();
            return NoContent();
        }
        [HttpDelete("{Email}")]
        public async Task<IActionResult> DeleteUser(string Email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                    return Problem(errors);
                }

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
