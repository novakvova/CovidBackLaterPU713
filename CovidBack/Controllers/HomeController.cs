using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CovidBack.DTO;
using CovidBack.Enities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CovidBack.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        public HomeController(ApplicationDBContext context,
            IHostingEnvironment env,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            long id = long.Parse(User.Claims.ToList()[0].Value);
            string domain = (string)_configuration.GetValue<string>("BackendDomain");
            var user = _context.Users.
                Select(u => new
                {
                    u.Id,
                    u.Email,
                    Image = $"{domain}android/{u.UserProfile.Image}",
                    Name = $"{u.UserProfile.Lastname} {u.UserProfile.Firstname}",
                    u.UserProfile.BirthDate,
                    u.UserProfile.Address,
                    u.UserProfile.Phone
                })
                .SingleOrDefault(x => x.Id == id);

            return Ok(user);
        }

        [HttpGet("profile/edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditUserProfile()
        {
            long id = long.Parse(User.Claims.ToList()[0].Value);
            string domain = (string)_configuration.GetValue<string>("BackendDomain");
            var user = _context.Users.
                Select(u => new
                {
                    u.Id,
                    u.Email,
                    Image = $"{domain}android/{u.UserProfile.Image}",
                    Name = $"{u.UserProfile.Lastname} {u.UserProfile.Firstname}",
                    u.UserProfile.BirthDate,
                    u.UserProfile.Address,
                    u.UserProfile.Phone
                })
                .SingleOrDefault(x => x.Id == id);
            return Ok(user);
        }

        [HttpPost("profile/edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromBody]UserEditDTO model)
        {
            long id = long.Parse(User.Claims.ToList()[0].Value);
            var item = _context.Users.Select(g=>g)
                .Include(u=>u.UserProfile)
                .SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                if (model.imageBase64 != null)
                {
                    var imageName = item.UserProfile.Image;
                    string savePath = _env.ContentRootPath;
                    string folderImage = "images";
                    savePath = Path.Combine(savePath, folderImage);
                    savePath = Path.Combine(savePath, imageName);
                    try
                    {
                        byte[] byteBuffer = Convert.FromBase64String(model.imageBase64);
                        using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
                        {
                            memoryStream.Position = 0;
                            using (Image imgReturn = Image.FromStream(memoryStream))
                            {
                                memoryStream.Close();
                                byteBuffer = null;
                                var bmp = new Bitmap(imgReturn);
                                bmp.Save(savePath, ImageFormat.Jpeg);
                            }
                        }
                    }
                    catch
                    {
                        return BadRequest(new
                        {
                            invalid = "Помилка обробки фото"
                        });
                    }


                }
                item.Email = model.Email;
                item.UserProfile.Lastname = model.Lastname;
                item.UserProfile.Firstname = model.Firstname;
                item.UserProfile.Address = model.Address;
                item.UserProfile.BirthDate = model.BirthDate;
                item.UserProfile.Phone = model.Phone;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new
                {
                    invalid = "Користувача немає"
                });
            }
        }
    }
}