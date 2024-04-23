using API_Project.Models;
using API_Project.Repository.Interfaces;

namespace API_Project.Repository
{
    public class AuthorizationRepository: IAuthorization
    {
        private readonly BrandContext _brandContext;
        public AuthorizationRepository (BrandContext brandContext)
        {
            _brandContext = brandContext;
        }
        public async Task<Authentication> GetUserAsync(string UserName)
        {
            Authentication user = null;
            user =await _brandContext.Authentications.FindAsync(UserName);
            return user;
        }

        public Task<Authentication> UserName(int userid)
        {
            throw new NotImplementedException();
        }
    }
}
