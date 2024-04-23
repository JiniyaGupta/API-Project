using Microsoft.AspNetCore.Http;
using API_Project.Models;
using API_Project.Repository;
using System.Text;
using API_Project.Repository.Interfaces;


namespace API_Project.Authorization
{
    public class AuthorizationMiddleware : IMiddleware
    {
        private readonly IAuthorization _authorizationRepository;

        public AuthorizationMiddleware(IAuthorization authorizationRepository)
        {
            _authorizationRepository = authorizationRepository;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            string ID = httpContext.User.Claims.First(c => c.Type == "Id").Value;
            int userid = int.Parse(ID);
            Authentication user = await _authorizationRepository.UserName(userid);
            if (user == null)
            {
                httpContext.Response.StatusCode = 401;
                httpContext.Response.WriteAsync("Unauthorized");
            }
            else await next(httpContext);
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}



