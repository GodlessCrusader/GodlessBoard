using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GodlessBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly MyDbContext _context;
        public UserController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public User Get()
        {
            return new User();
           
        }

        [HttpPost]
        public async Task Post([FromForm] IFormFile file)
        {
            try
            {
                var path = "";
                using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                {

                    await file.CopyToAsync(fs);

                    var userName = Auth.GetUserName(User.Identity.Name);
                    var user = (from u in _context.Users
                                where u.UserName == userName
                                select u).SingleOrDefault();
                    var media = new Media()
                    {
                        Name = file.Name,
                        Type = Models.MediaType.Image,
                        Weight = file.Length,
                        Owner = user
                    };
                    _context.Media.Add(media);

                    user.Medias.Add(media);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }


        }
    }
}
