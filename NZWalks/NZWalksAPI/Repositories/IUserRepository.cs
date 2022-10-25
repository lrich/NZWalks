using Microsoft.IdentityModel.Tokens;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string user, string password);
    }
}
