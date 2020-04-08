using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        public ProductsController(ApplicationDBContext context,
            IConfiguration configuration,
            IHostingEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }
        public IActionResult GetAll()
        {
            string domain = (string)_configuration.GetValue<string>("BackendDomain");

            var model = _context.Products
                .Select(p => new ProductDTO
                {
                    title = p.Title,
                    price = p.Price,
                    url = $"{domain}android/{p.Url}"
                }).ToList();
            Thread.Sleep(2000);
            //List<ProductDTO> model = new List<ProductDTO>() {

            //    new ProductDTO{
            //        title = "Salo",
            //        price = "99999",
            //        url = "http://10.0.2.2/android/1.png"
            //    }
            //};
            return Ok(model);
        }
        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromRoute] int id)
        {
            var item = _context.Products.SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                ProductEditDTO product = new ProductEditDTO()
                {
                    Id = item.Id,
                    price = item.Price.ToString(),
                    title = item.Title
                };
                return Ok(product);
            }
            else
            {
                return BadRequest(new
                {
                    invalid = "Не знайдено по даному id"
                });
            }
        }
        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateByIdProductForEdit([FromBody]ProductEditDTO model)
        {
            var item = _context.Products.SingleOrDefault(x => x.Id == model.Id);
            if (item != null)
            {
                item.Title = model.title;
                item.Price = model.price;
                //_context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new
                {
                    invalid = "Не знайдено по даному id"
                });
            }
        }
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody]ProductCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    invalid = "Не валідна модель"
                });
            }
            var imageName = Path.GetRandomFileName() + ".jpg";
            string savePath = _env.ContentRootPath;
            string folderImage = "images";
            savePath = Path.Combine(savePath, folderImage);
            savePath = Path.Combine(savePath, imageName);
            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                byte[] byteBuffer = Convert.FromBase64String(model.imageBase64);
                fs.Write(byteBuffer);
            }

            //var faker = new Faker();
            Product product = new Product
            {
                Title = model.title,
                Url = imageName,
                Price = model.price
            };
            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok(
            new
            {
                id = product.Id
            });
        }

    }
}