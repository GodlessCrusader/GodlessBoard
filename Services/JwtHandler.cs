using GodlessBoard.Data;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GodlessBoard.Services
{
    public class JwtHandler
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public JwtHandler( MyDbContext myDbContext, IConfiguration configuration)
        {
            _dbContext = myDbContext;
            _configuration = configuration;
        }

        public string GenetarateToken(User user)
        {
            var result = string.Empty;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Name, user.DisplayName)
            };
            
            //var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            //ClaimsPrincipal claimsPrincipal = new(identity);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                signingCredentials: creds,
                claims: claims.ToArray(),
                expires: DateTime.Now.AddDays(1));

            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            result = jwtSecurityHandler.WriteToken(token);
            return result;
        }

        public User? GetUserByToken(string token)
        {
            var validationParams = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
            SecurityToken? secToken = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, validationParams, out secToken);
            }
            catch
            {

            }
            if (secToken == null)
                return null;
            var jwt = new JwtSecurityToken(token);
            var userLogin = jwt.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var user = _dbContext.Users.First(x => x.UserName == userLogin);
            return user;
            
        }
       
    }
}
