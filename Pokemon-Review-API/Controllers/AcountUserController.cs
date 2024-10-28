using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.Helper;
using ProjectCRUD.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectCRUD.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AcountUserController(JwtOption jwtOption, ApplictionDBCotext dBCotext) : ControllerBase
    {
        [HttpPost]
        [Route("auth")]
        public ActionResult<string> AuthenticationUser(AuthenticationRequest request)
        {
            var use = dBCotext.Set<User>().FirstOrDefault(x => x.Name == request.UserName &&
            x.Paswored == request.Password);
            if (use == null)
            {
                return Unauthorized();
            }
            var tokenHander = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOption.Issuer,
                Audience = jwtOption.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOption.SigningKey)), SecurityAlgorithms.HmacSha256),
                 Subject = new ClaimsIdentity(new Claim[]
                 {
                     new(ClaimTypes.NameIdentifier,use.Id.ToString() ),
                     new(ClaimTypes.Name,use.Name ),
                     new(ClaimTypes.Role,"admin"),
                     new(ClaimTypes.Role,"SuperUser")
                 })
            };
            var securitytoken = tokenHander.CreateToken(tokenDescriptor);
            var accessToken = tokenHander.WriteToken(securitytoken);
            return Ok(accessToken);
        }
    }
}
