using API_Project.Models;

namespace API_Project.Repository
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetOrderProducts();
        Task<Order> GetOrderProductById(Guid id);
        Task<Order> CreateOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Guid id);

        Task<Brand> GetMostSoldProductAsync();
    }
}