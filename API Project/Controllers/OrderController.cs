using API_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BrandContext _dbContext;
        public OrderController(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetBrands()
        {
            if (_dbContext.Orders == null)
            {
                return NotFound();
            }
            return await _dbContext.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            if (_dbContext.Orders == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost("orderProduct")]
        public async Task<ActionResult<Order>> PostBrand(Order order)
        {
            // _dbContext.Orders.Add(order);
            //  await _dbContext.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
            Order orderpro = new Order();
            orderpro.OrderId = Guid.NewGuid();
            orderpro.CustomerId = order.CustomerId;
            orderpro.ID = order.ID;
            orderpro.quantity = order.quantity;
            _dbContext.Orders.Add(orderpro);
            await _dbContext.SaveChangesAsync();
            var gotorder = await _dbContext.Brands.FindAsync(order.ID);
            if (gotorder == null)
            {
                return NotFound();
            }

            gotorder.Quantity -= order.quantity;
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = orderpro.OrderId }, orderpro);
        }

        [HttpPut]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            _dbContext.Entry(order).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        private bool OrderAvailable(Guid id)
        {
            return (_dbContext.Orders?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            if (_dbContext.Orders == null)
            {
                return NotFound();
            }

            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
