using GodlessBoard.Data;
using GodlessBoard.Pages.Account;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace GodlessBoard.Services
{
    public class HashGenerator
    {
        private readonly IConfiguration _configuration;
        public string Jwt { get; set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public HashGenerator()
        {

        }
        public HashGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void GenerateHash(string password)
        {
            using(var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public void GenerateHash(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                PasswordSalt = salt;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public void GenerateJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );
            Jwt = new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool VerifyUserData(User user, Credential credential)
        {
            GenerateHash(credential.Password, user.PasswordSalt);
            return (user.PasswordHash.SequenceEqual(this.PasswordHash));
               
        }
    }
}
