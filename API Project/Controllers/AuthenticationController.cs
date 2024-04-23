using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using API_Project.Models;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
            private IConfiguration _config;
            private BrandContext _appDbContext;
            public AuthenticationController(IConfiguration config, BrandContext appDbContext)
            {
                _config = config;
                _appDbContext = appDbContext;
            }
           // [AllowAnonymous]
            [HttpPost]
            [Route("/Login")]
            public IActionResult Login(string username, string Password)
            {
                var user = AuthenticateUser(username, Password);
                if (user == null)
                {
                    return NotFound();
                }
              
                    var tokenString = GenerateJSONWebToken(user);
                    return Ok(new { token = tokenString });
            }

            private string GenerateJSONWebToken(Authentication userInfo)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[] {
    new Claim(JwtRegisteredClaimNames.Name, userInfo.UserName),
    new Claim(JwtRegisteredClaimNames.Sub,userInfo.Password),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            private Authentication AuthenticateUser(string username, string password)
            {
                Authentication user = null;
                Authentication users = (Authentication)_appDbContext.Authentications.Where(lo => lo.UserName == username).Select(pro => pro).Single();
                if (users.Password != password)
                {
                    return null;
                }
                if (users != null)
                {
                    user = new Authentication { UserName = users.UserName, Password = users.Password };
                }
                return user;
            }
        }
    }

