using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Passwordmanager.Data;
using Passwordmanager.Model;
using Passwordmanager.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Passwordmanager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ITokenService _tokenService;
        public IConfiguration _configuration;

        public TokenController(ApplicationDbContext context, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenApiModel tokenApiModel)
        {

            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            string? refreshToken = tokenApiModel.RefreshToken;

            var user = _context.UserInfos.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid refresh token");
            



            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                    };


            var newAccessToken = _tokenService.GenerateAccessToken(claims);

            return Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = refreshToken
            });



            //    string? accessToken = Request.Cookies["access_token"];
            //    string? refreshToken = tokenApiModel.RefreshToken;

            //var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            //var username = principal.Identity.Name; //this is mapped to the Name claim by default

            //var user = _context.UserInfos.SingleOrDefault(u => u.UserName == username);

            //    if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            //        return BadRequest("Invalid client request");


            //var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            //var newRefreshToken = _tokenService.GenerateRefreshToken();
            //user.RefreshToken = newRefreshToken;

            //    _context.SaveChanges();

            //    return Ok(new AuthenticatedResponse()
            //{
            //    Token = newAccessToken,
            //        RefreshToken = newRefreshToken
            //    });
            //}

        }
    }

}