using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CovidBack.DTO;
using CovidBack.Enities;
using Microsoft.AspNetCore.Authorization;
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
        public ProductsController(ApplicationDBContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            Random r = new Random();
            //var faker = new Faker();
            Product product = new Product
            {
                Title = model.title,
                Url = r.Next(1, 10).ToString() + ".jpg",
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