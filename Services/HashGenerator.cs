using GodlessBoard.Models;
using GodlessBoard.Pages.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace GodlessBoard.Services
{
    public class HashGenerator
    {
        private readonly IConfiguration _configuration;
        
        public HashGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void GenerateHash(string password, ref byte[] passwordSalt, out byte[] passwordHash )
        {
            if(passwordSalt == null)
                using(var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            else
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                   passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
        }
        
        public bool VerifyUserData(User user, Credential credential)
        {
            byte[] passwordHash;
            var passwordSalt = user.PasswordSalt;
            GenerateHash(credential.Password, ref passwordSalt, out passwordHash);
            return (user.PasswordHash.SequenceEqual(passwordHash));
               
        }
    }
}
