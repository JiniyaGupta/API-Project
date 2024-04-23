using API_Project.Repository;
using Microsoft.EntityFrameworkCore;
using API_Project.Models;
using System;


namespace API_Project.Repository
{
    public class ProductRepository : IProduct
    {
        private readonly BrandContext _dbContext;

        public ProductRepository(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Brand>> GetAllProducts()
        {
            return await _dbContext.Brands.ToListAsync();
        }

        public async Task<Brand> GetProductById(Guid id)
        {
            return await _dbContext.Brands.FindAsync(id);
        }

        public async Task AddProduct(Brand product)
        {
            product.ID = Guid.NewGuid();
            _dbContext.Brands.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Brand product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            var product = await _dbContext.Brands.FindAsync(id);
            if (product != null)
            {
                _dbContext.Brands.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}