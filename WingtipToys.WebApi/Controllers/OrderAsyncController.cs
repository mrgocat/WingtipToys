using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.BusinessLogicLayer.Services;

namespace WingtipToys.WebApi.Controllers
{
    [Route("api/v2/order")]
    [ApiController]
    [Authorize]
    public class OrderAsyncController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderAsyncController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            string userId = getUserId();
            if (userId == null)
            {
                return Unauthorized("Authentication Information required.");
            }

            //string userName = null;
            return Ok(await _orderService.GetOrderListAsync(userId));
        }
        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetAsync(int orderId)
        {
            string userId = getUserId();
            if (userId == null)
            {
                return Unauthorized("Authentication Information required.");
            }

            OrderDto dto = await _orderService.GetAsync(orderId);
            if(dto == null)
            {
                return NotFound();
            }
            if(dto.Username != userId)
            {
                return NotFound();
            }
            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody]OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = getUserId();
            if (userId == null)
            {
                return Unauthorized("Authentication Information required.");
            }
            dto.Username = userId;

            int orderId = 0;
            try
            {
                orderId = await _orderService.AddAsync(dto);
            }catch(KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            return Created($"api/vi/order/{orderId}", orderId);
        }
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteAsync(int orderId)
        {
            await _orderService.DeleteAsync(orderId);
            return NoContent();
        }

        private string getUserId()
        {
            var claimId = this.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimId == null)
            {
                return null;
            }
            return claimId.Value;
        }
    }
}
