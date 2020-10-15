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
    [Route("api/v2/Product")]
    [ApiController]
    public class ProductAsyncController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductAsyncController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery]string name = null, [FromQuery] string value = null)
        {
            if(name == null || value == null)
            {
                return Ok(await _productService.GetProductListAsync());
            }
            else
            {
                if(name == "category")
                {
                    int category = 0;
                    bool result = Int32.TryParse(value, out category);
                    if(!result) return BadRequest();
                    
                    return Ok(await _productService.GetProductListbyCategoryAsync(category));
                }else if(name == "name")
                {
                    return Ok(await _productService.SearchProductsAsync(value));
                }
                else
                {
                    return BadRequest();
                }           
            }
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult> GetAsync(int productId)
        {
            ProductDto dto = null;
            try
            {
                dto = await _productService.GetAsync(productId);
            }catch(KeyNotFoundException)
            {
                return NotFound();
            }
            return Ok(dto);
        }
        [HttpGet("category")]
        public async Task<ActionResult> GetProductCategoryAsync()
        {
            return Ok(await _productService.GetCategoryListAsync());
        }
    }
}
