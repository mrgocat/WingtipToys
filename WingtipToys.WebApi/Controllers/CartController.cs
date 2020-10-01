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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{cartId}")]
        public ActionResult Get(string cartId) // client maintain shopping cart ID.
        {
            return Ok(_cartService.Get(cartId));
        }
        [HttpGet]
        public ActionResult Get() // server has cart id
        {
            string cartId = null;
            // TO DO assign cart Id from key 
            return Ok(_cartService.Get(cartId));
        }
        [HttpPost]
        public ActionResult Add([FromBody]CartItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string cartId = null;
            try
            {
                cartId = _cartService.Add(dto);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            
            return Created($"api/vi/cart/{cartId}", new { CartId = cartId}); // not properly working with Angular
        //    return Ok(cartId);
        }
        [HttpPatch]
        public ActionResult Patch([FromBody] CartItemDto dto)
        {
            string cartId = dto.CartId;
            string itemId = dto.Id;
            int quantity = dto.Quantity;
            if (cartId == null || itemId == null || quantity <= 0)
            {
                return BadRequest("Not enough information to update.");
            }
            _cartService.Patch(cartId, itemId, quantity);
            return NoContent();
        }
        [HttpDelete("{cartId}")]
        public ActionResult Delete(string cartId)
        {
            _cartService.Delete(cartId);
            return NoContent();
        }
        [HttpDelete("{cartId}/{itemId}")]
        public ActionResult Delete(string cartId, string itemId)
        {
            _cartService.Delete(cartId, itemId);
            return NoContent();
        }
    }
}
