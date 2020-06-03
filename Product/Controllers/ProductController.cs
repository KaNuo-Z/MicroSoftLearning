using Microsoft.AspNetCore.Mvc;
using Product.Model;
using System;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet()]
        public ProductModel Get(string id)
        {
            return new ProductModel()
            {
                Id = this.HttpContext.Request.Host.Host + this.HttpContext.Request.Host.Port,
                Name = "Product",
                Price = 100M,
                Discount = 8.0,
                CreateTime = DateTime.Now,
                OverTime = DateTime.Now.AddDays(60)
            };
        }
    }
}