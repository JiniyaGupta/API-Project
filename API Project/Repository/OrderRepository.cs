using Microsoft.EntityFrameworkCore;
using API_Project.Models;
using API_Project.Repository;
using Microsoft.Extensions.Caching.Memory;
using API_Project.Cache;
using LazyCache;


namespace API_Project.Repository
{
    public class OrderRepository : IOrder
    {
        private readonly BrandContext _dbContext;
        private readonly IMemoryCache _cacheProvider;


        public OrderRepository(BrandContext dbContext,IMemoryCache cacheProvider)
        {
            _dbContext = dbContext;
            _cacheProvider = _cacheProvider;
        }

        public async Task<IEnumerable<Order>> GetOrderProducts()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderProductById(Guid id)
        {
            return await _dbContext.Orders.FindAsync(id);
        }

        public async Task<Order> CreateOrder(Order order)
        {
            order.OrderId = Guid.NewGuid();
            _dbContext.Orders.Add(order);
            var product = await _dbContext.Brands.FindAsync(order.ID);
            product.Quantity -= order.quantity;
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderProductExists(order.OrderId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private bool OrderProductExists(Guid id)
        {
            return _dbContext.Orders.Any(e => e.OrderId == id);
        }

        public async Task<Brand> GetMostSoldProductAsync()
        {
            if (_cacheProvider.TryGetValue(CacheKeys.Product, out Brand products))
                return products;

            var mostSoldProduct = await _dbContext.Orders
                 .GroupBy(o => o.ID)
                 .Select(g => new
                 {
                     ID = g.Key,
                     totalquantity = g.Sum(o => o.quantity)
                 }).ToListAsync();

            var maxquantity = mostSoldProduct.OrderByDescending(g => g.totalquantity).FirstOrDefault();

            var product = _dbContext.Brands.Where(p => p.ID == maxquantity.ID).Select(p => p).Single();
            //products = await _orderProductRepository.GetTopSellingProductAsync();
            var cacheEntryOption = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(30),
                Size = 1024
            };
            _cacheProvider.Set(CacheKeys.Product, product, cacheEntryOption);
            _cacheProvider.Remove(CacheKeys.Product);


            return products;
        }
    }
}