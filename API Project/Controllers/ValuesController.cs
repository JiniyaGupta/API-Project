using API_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API_Project.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productRepository;

        public ProductController(IProduct productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<Brand>> GetProductById(Guid id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult<Brand>> AddProduct(Brand product)
        {
            await _productRepository.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ID }, product);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Brand product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }
            await _productRepository.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productRepository.DeleteProduct(id);
            return NoContent();
        }
    }
}