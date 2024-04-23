using API_Project.Models;

namespace API_Project.Repository.Interfaces
{
    public interface IAuthorization
    {
        Task<Authentication> GetUserAsync(string UserName);
        Task<Authentication> UserName(int userid);
    }
}
