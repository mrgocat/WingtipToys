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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public ActionResult Get()
        {
            string userName = null;
            return Ok(_orderService.GetOrderList(userName));
        }
        [HttpGet("{orderId}")]
        public ActionResult Get(int orderId)
        {
            OrderDto dto = _orderService.Get(orderId);
            if(dto == null)
            {
                NotFound();
            }
            return Ok(dto);
        }
        [HttpPost]
        public ActionResult Add([FromBody]OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int orderId = 0;
            try
            {
                orderId = _orderService.Add(dto);
            }catch(KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            return Created($"api/vi/order/{orderId}", orderId);
        }
        [HttpDelete("{orderId}")]
        public ActionResult Delete(int orderId)
        {
            _orderService.Delete(orderId);
            return NoContent();
        }
    }
}
