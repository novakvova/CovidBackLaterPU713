using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidBack.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidBack.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        public IActionResult GetAll()
        {
            List<ProductDTO> model = new List<ProductDTO>() {

                new ProductDTO{
                    title = "Salo",
                    price = "99999",
                    url = "http://10.0.2.2/android/1.png"
                }
            };
            return Ok(model);
        }
    }
}