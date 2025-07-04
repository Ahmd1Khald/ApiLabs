using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APISols.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(Regiter _userFromReq)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = _userFromReq.Username,
                    Email = _userFromReq.Email
                };

                var result = await _userManager.CreateAsync(user, _userFromReq.Password);
                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(user, "User");
                    return Ok("User Created Successfully");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest("Invalid Data");

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login _userFromReq)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(_userFromReq.Username);
                if (user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, _userFromReq.Password);
                    if (result)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            //new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.Id)
                        };
                        var userRoles = await _userManager.GetRolesAsync(user);
                        foreach (var role in userRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        //siging credentials
                        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("AhmedKhaled1234565otifyklasjasdlkl;asdas"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                        JwtSecurityToken token = new JwtSecurityToken(

                            issuer: "http://localhost:5237",//API
                            audience: "http://localhost:5151",//MVC
                            expires: DateTime.Now.AddMinutes(100),
                            claims: claims,
                            signingCredentials: creds
                            );


                        //generate token string
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(new
                        {
                            token = tokenString,
                            userName = user.UserName,
                            email = user.Email
                        });


                    }
                    else
                    {
                        return Unauthorized("Invalid Password");
                    }
                }
                else
                {
                    return NotFound("User Not Found");
                }
            }
            return BadRequest("Invalid Data");

        }

    }
}
