using Blazor.Extensions.Canvas.WebGL;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;
using System.Text;

namespace GodlessBoard.Services
{
    public static class MediaUploadRouter
    {
       
        public static async Task<Media> UploadMediaAsync(byte[] e, string ownerName, string oldFileName, string webRootPath, string key)
        {
            
            string uploadPath;
            StringBuilder relativePath = new StringBuilder();
            var userDirectories = ($"{GeneratePath(e, ownerName, key)}{Path.GetExtension(oldFileName)}").Split('\\');
            
            relativePath.Append("../upload/userMedia/");
            relativePath.Append(userDirectories[0]);
            relativePath.Append('/');
            relativePath.Append(userDirectories[1]);
            
            uploadPath = Path.Combine(webRootPath,
                "upload",
                "userMedia",
                $"{GeneratePath(e, ownerName, key)}{Path.GetExtension(oldFileName)}");
            
            if (!Directory.Exists(uploadPath.Replace(userDirectories[1], string.Empty)))
                Directory.CreateDirectory(uploadPath.Replace(userDirectories[1], string.Empty));
            
            if (!File.Exists(uploadPath))
            {
                using (var fs = new FileStream(uploadPath, FileMode.Create))
                {
                    fs.Write(e, 0, e.Length);
                } 
            }

            return new Media() {
                UserDisplayName = oldFileName,
                Name = relativePath.ToString(),
                Type = MediaType.Image,
            };

            
            

            
        }

        public static bool CheckExistance(byte[] e, string ownerName, string oldFileName, string webRootPath, string key)
        {
            var uploadPath = Path.Combine(webRootPath,
                "upload",
                "userMedia",
                $"{GeneratePath(e, ownerName, key)}{Path.GetExtension(oldFileName)}");
            return File.Exists(uploadPath);
        }
        private static string GeneratePath(byte[] mediaBytes, string ownerName, string key)
        {
            if (mediaBytes == null || ownerName == null)
                throw new NullReferenceException();
            else
            {
                string uploadPath;
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
                {
                    var ownerNameHash = (hmac.ComputeHash(Encoding.UTF8.GetBytes(ownerName))).ToHex(false);
                    var mediaNameHash = (hmac.ComputeHash(mediaBytes)).ToHex(false);
                    uploadPath = Path.Combine(ownerNameHash, mediaNameHash); 
                }
                
                return uploadPath;
            }
            
        }

        private static string ToHex(this byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }



    }
}
