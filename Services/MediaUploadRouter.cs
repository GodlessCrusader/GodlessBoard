using Blazor.Extensions.Canvas.WebGL;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace GodlessBoard.Services
{
    public class MediaUploadRouter
    {
        private ILogger<MediaUploadRouter> _logger;
        private IConfiguration _configuration;
        private IWebHostEnvironment _env;
        public MediaUploadRouter(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, ILogger<MediaUploadRouter> logger)
        {
            _env = webHostEnvironment;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<Media> UploadMediaAsync(byte[] e, string ownerName, string oldFileName)
        {
            var key = _configuration.GetSection("AppSettings:Key").Value;
            string uploadPath;
            StringBuilder relativePath = new StringBuilder();
            var userDirectories = GeneratePath(e, ownerName, key);

            relativePath.Append("../upload/userMedia/");
            relativePath.Append(userDirectories[0]);
            relativePath.Append('/');
            relativePath.Append(userDirectories[1]);
            relativePath.Append(Path.GetExtension(oldFileName));
            
            uploadPath = Path.Combine(_env.WebRootPath,
                "upload",
                "userMedia",
                userDirectories[0],
                $"{userDirectories[1]}{Path.GetExtension(oldFileName)}");
            _logger.LogInformation($"upload path from MediaUploadRouter: {uploadPath}");
            if (!Directory.Exists(uploadPath.Replace(userDirectories[1], string.Empty)))
                Directory.CreateDirectory(uploadPath.Replace(userDirectories[1], string.Empty));
            
            if (!File.Exists(uploadPath))
            {
                _logger.LogInformation($"File doesn't exist. Writing");
                using (var fs = new FileStream(uploadPath, FileMode.Create))
                {
                    fs.Write(e, 0, e.Length);
                }
                _logger.LogInformation($"File download is successful");
            }

            return new Media() {
                UserDisplayName = oldFileName,
                Name = relativePath.ToString(),
                Type = MediaType.Image
            };

        }

       public Task DeleteAsync(Media media)
       {
            var folders = media.Name.Replace("..", string.Empty).Split('/');
            var path = Path.Combine(folders);
            path = Path.Combine(_env.WebRootPath, path);
            if (File.Exists(path))
            {
                File.Delete(path);
            } 
            return Task.CompletedTask;
       }
        
        public bool CheckExistance(byte[] e, string ownerName, string oldFileName)
        {
            var key = _configuration.GetSection("AppSettings:Key").Value;
            var uploadPath = Path.Combine(_env.WebRootPath,
                "upload",
                "userMedia",
                $"{GeneratePath(e, ownerName, key)}{Path.GetExtension(oldFileName)}");
            return File.Exists(uploadPath);
        }

       
        private static List<string> GeneratePath(byte[] mediaBytes, string ownerName, string key)
        {
            if (mediaBytes == null || ownerName == null)
                throw new NullReferenceException();
            else
            {
                List<string> uploadPath = new();
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
                {
                    var ownerNameHash = (hmac.ComputeHash(Encoding.UTF8.GetBytes(ownerName))).ToHex(false);
                    var mediaNameHash = (hmac.ComputeHash(mediaBytes)).ToHex(false);
                    uploadPath.Add(ownerNameHash);
                    uploadPath.Add(mediaNameHash);
                }
                
                return uploadPath;
            }
            
        }
    }
    
}
