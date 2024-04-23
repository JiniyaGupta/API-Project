using API_Project.Models;

namespace API_Project.Repository
{
    public interface IProduct
    {
        Task<IEnumerable<Brand>> GetAllProducts();
        Task<Brand> GetProductById(Guid id);
        Task AddProduct(Brand product);
        Task UpdateProduct(Brand product);
        Task DeleteProduct(Guid id);
    }
}