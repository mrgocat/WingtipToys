using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.BusinessLogicLayer.Services;

namespace WingtipToys.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public ActionResult Get([FromQuery]string name = null, [FromQuery] string value = null)
        {
            if(name == null || value == null)
            {
                return Ok(_productService.GetProductList());
            }
            else
            {
                if(name == "category")
                {
                    int category = 0;
                    bool result = Int32.TryParse(value, out category);
                    if(!result) return BadRequest();
                    
                    return Ok(_productService.GetProductListbyCategory(category));
                }else if(name == "name")
                {
                    return Ok(_productService.SearchProducts(value));
                }
                else
                {
                    return BadRequest();
                }           
            }
            
        }
        [HttpGet("{productId}")]
        public ActionResult Get(int productId)
        {
            ProductDto dto = null;
            try
            {
                dto = _productService.Get(productId);
            }catch(KeyNotFoundException)
            {
                return NotFound();
            }
            return Ok(dto);
        }
        [HttpGet("category")]
        public ActionResult GetProductCategory()
        {
            return Ok(_productService.GetCategoryList());
        }
    }
}
