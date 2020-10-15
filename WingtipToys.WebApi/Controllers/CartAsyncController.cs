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
    [Route("api/v2/Cart")]
    [ApiController]
    public class CartAsyncController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartAsyncController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{cartId}")]
        public async Task<ActionResult> GetAsync(string cartId) // client maintain shopping cart ID.
        {
            return Ok(await _cartService.GetAsync(cartId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAsync() // server has cart id
        {
            string cartId = null;
            // TO DO assign cart Id from key 
            return Ok(await _cartService.GetAsync(cartId));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CartItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                //    return BadRequest(ModelState);
                return ValidationProblem(ModelState);
            }
            string cartId = null;
            try
            {
                cartId = await _cartService.AddAsync(dto);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Created($"api/vi/cart/{cartId}", new { CartId = cartId });
            //   return CreatedAtRoute(nameof(GetAsync), new { id = cartId }, new { cartId = cartId };
            //   return Ok(cartId);
        }
        // Microsoft.aspNetCore.JsonPatch
        // Microsoft.AspNetCore.Mvc.NewtonsoftJson
        /*services.AddControllers().AddNewtonsoftJson(s => {
            s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });*/
        /*
        [HttpPatch("{id}")]
        public async [HttpPatch] Task<ActionResult> PatialUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc){
            var poco = await _cartService.GetAsync(cartId);
            if(poco == null) return NotFound();
            var cartToPatch = _mapper.Map<CartUpdateDto>(poco);
            patchDoc.ApplyTo(cartToPath, ModelState);
            if(!TryValidateModel(cartToPatch)){
                return ValidationProblem(ModelState);
            }
            _mapper.Map(cartUpdateDto, poco);
            _cartService.Update(poco);
            return NoContext();
        }*/
        /* request body info... 
        [
            {
                "op": "replace",
                "path": "/quantity",
                "value": 3
            }
        ]*/
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAsync([FromBody] CartItemDto dto)
        {
            string cartId = dto.CartId;
            string itemId = dto.Id;
            int quantity = dto.Quantity;
            if (cartId == null || itemId == null || quantity <= 0)
            {
                return BadRequest("Not enough information to update.");
            }
            await _cartService.PatchAsync(cartId, itemId, quantity);
            return NoContent();
        }
        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteAsync(string cartId)
        {
            await _cartService.DeleteAsync(cartId);
            return NoContent();
        }
        [HttpDelete("{cartId}/{itemId}")]
        public async Task<ActionResult> DeleteAsync(string cartId, string itemId)
        {
            await _cartService.DeleteAsync(cartId, itemId);
            return NoContent();
        }
    }
}
