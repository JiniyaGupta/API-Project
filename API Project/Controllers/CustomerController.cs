using API_Project.Models;
using Microsoft.AspNetCore.Mvc;
using API_Project.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using API_Project.Authorization;
using Microsoft.AspNetCore.Identity;
using AuthorizationMiddleware = API_Project.Authorization.AuthorizationMiddleware;

namespace API_Project.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    //[ServiceFilter(typeof(AuthorizationMiddleware))]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customerRepository;

        public CustomerController(ICustomer customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet ("GetAllCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("GetCustomerByID")]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> PostCustomer(Customer customer)
        {
            await _customerRepository.AddCustomer(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("ModifyCustomer")]
        public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            await _customerRepository.UpdateCustomer(customer);
            return NoContent();
        }

        [HttpDelete("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            await _customerRepository.DeleteCustomer(id);
            return NoContent();
        }
    }
}