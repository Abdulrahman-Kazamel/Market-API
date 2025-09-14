using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Data;
using Api.Helpers;
using Api.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;



namespace Api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UsersController(JWTOptoionsHelper _jwtOptoionsHelper, ApplicationDbContext dbContext) : ControllerBase
    {

        [HttpPost]
        [Route("auth")]
        public ActionResult<string> AuthenticationUser(AuthenticationRequest authenticationRequest)
        {
            var user = dbContext.users.FirstOrDefault(u => u.Name == authenticationRequest.UserName && u.Password == authenticationRequest.Password);
            if(user == null)
                return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtOptoionsHelper.Issuer,
                Audience = _jwtOptoionsHelper.Audience,

                SigningCredentials = new SigningCredentials(
                     new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(_jwtOptoionsHelper.SigninKey)),
                     SecurityAlgorithms.HmacSha256),

                Subject = new ClaimsIdentity(new Claim[]{
                              new(ClaimTypes.NameIdentifier, user.Id.ToString() ) ,
                              new(ClaimTypes.Name, user.Name)

                          })
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(accessToken);
        }
    }
}
