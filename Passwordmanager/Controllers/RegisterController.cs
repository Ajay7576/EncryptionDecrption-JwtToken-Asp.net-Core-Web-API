using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Passwordmanager.Data;
using Passwordmanager.Model;
using Passwordmanager.Services;
using Passwordmanager.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Passwordmanager.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IConfiguration _configuration;

        private readonly ITokenService _tokenService;
        public RegisterController(IConfiguration config, ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _configuration = config;
            _tokenService = tokenService;


        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> register([FromBody] UserInfo register)
        {
            if (register == null)
                return BadRequest();
            await _context.UserInfos.AddAsync(register);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [Route("login")]
        //[Authorize]

        public async Task<IActionResult> login(LoginViewModel loginViewModel)
        {
            if (loginViewModel == null)
                return BadRequest("wrong user password");

            var user = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserName == loginViewModel.UserName && x.Password == loginViewModel.Password);
            if (user == null)
                return BadRequest("invalid");

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("UserName", user.UserName),
                        new Claim("Password", user.Password)
                    };


            var accessToken = _tokenService.GenerateAccessToken(claims);
            //HttpContext.Response.Cookies.Append("access_token", accessToken, new CookieOptions { HttpOnly = true });

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _context.SaveChanges();

            return Ok(new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });


            // also add cookie auth 
            //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //identity.AddClaim(new Claim("Id", user.Id.ToString()));
            //identity.AddClaim(new Claim("UserName", user.UserName));
            //identity.AddClaim(new Claim("Password", user.Password));


            //var principal = new ClaimsPrincipal(identity);

            //await HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    principal,

            //    new AuthenticationProperties
            //    {
            //        IsPersistent = true,
            //        AllowRefresh = true,
            //        ExpiresUtc = DateTime.UtcNow.AddSeconds(60)
            //    });
            
        }

        [HttpGet]
       // [Authorize]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await _context.UserInfos.ToListAsync());
        }


        



    }
}
