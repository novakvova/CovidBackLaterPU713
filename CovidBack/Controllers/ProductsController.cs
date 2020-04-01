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
                .Select(p=>new ProductDTO
                {
                    title=p.Title,
                    price=p.Price,
                    url= $"{domain}android/{p.Url}"
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
    }
}