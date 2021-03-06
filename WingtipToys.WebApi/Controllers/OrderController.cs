﻿using System;
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
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
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
            string userId = getUserId();
            if (userId == null)
            {
                return Unauthorized("Authentication Information required.");
            }

            //string userName = null;
            return Ok(_orderService.GetOrderList(userId));
        }
        [HttpGet("{orderId}")]
        public ActionResult Get(int orderId)
        {
            string userId = getUserId();
            if (userId == null)
            {
                return Unauthorized("Authentication Information required.");
            }

            OrderDto dto = _orderService.Get(orderId);
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
        public ActionResult Add([FromBody]OrderDto dto)
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

        private String getUserId()
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
