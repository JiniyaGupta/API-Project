using Microsoft.AspNetCore.Mvc;
using API_Project.Models;
using API_Project.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly IOrder _orderProductRepository;

        public OrderProductController(IOrder orderProductRepository)
        {
            _orderProductRepository = orderProductRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderProducts()
        {
            var orderProducts = await _orderProductRepository.GetOrderProducts();
            return Ok(orderProducts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderProduct(Guid id)
        {
            var orderProduct = await _orderProductRepository.GetOrderProductById(id);
            if (orderProduct == null)
            {
                return NotFound();
            }
            return orderProduct;
        }

        [HttpPost("Order")]
        public async Task<ActionResult<Order>> Order(Order orderProduct)
        {
            var createdOrderProduct = await _orderProductRepository.CreateOrder(orderProduct);
            return CreatedAtAction(nameof(GetOrderProduct), new { id = createdOrderProduct.OrderId }, createdOrderProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderProduct(Guid id, Order orderProduct)
        {
            if (id != orderProduct.OrderId)
            {
                return BadRequest();
            }

            var result = await _orderProductRepository.UpdateOrder(orderProduct);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderProduct(Guid id)
        {
            var result = await _orderProductRepository.DeleteOrder(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("Most-SoldProduct")]

        public async Task<IActionResult> GetMostSoldProductAsync()
        {
            try
            {
                var mostSoldProduct = await _orderProductRepository.GetMostSoldProductAsync();
                if(mostSoldProduct == null )
                {
                    return NotFound();
                }

                return Ok(mostSoldProduct);
            }

            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        
        
    }
}