using GodlessBoard.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;
using System.Text;

namespace GodlessBoard.Services
{
    public static class MediaUploadRouter
    {
        public static async Task<string> UploadMedia(byte[] e, string ownerName, string oldFileName, string webRootPath)
        {
            string mediaName = string.Empty;
            string uploadPath;
            uploadPath = Path.Combine(webRootPath,"upload","userMedia", $"{GeneratePath(e, ownerName)}{Path.GetExtension(oldFileName)}");
            
            mediaName = uploadPath.Split('\\').Last();
            if (!Directory.Exists(uploadPath.Replace(mediaName, string.Empty)))
                Directory.CreateDirectory(uploadPath.Replace(mediaName, string.Empty));
            if (!File.Exists(uploadPath))
            {
                using (var fs = new FileStream(uploadPath, FileMode.Create))
                {
                    await fs.WriteAsync(e, 0, e.Length);
                }
                return mediaName;
            }
            else
                return string.Empty;

            
            

            
        }

        private static string GeneratePath(byte[] mediaBytes, string ownerName)
        {
            if (mediaBytes == null || ownerName == null)
                throw new NullReferenceException();
            else
            {
                string uploadPath;
                using (var hmac = new HMACSHA256(System.Text.UTF8Encoding.UTF8.GetBytes("sas")))
                {
                    var ownerNameHash = (hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(ownerName))).ToHex(false);
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
