using IdentityServer_DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer_DAL.Repos.Contracts
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(string id);

        Task<User?> FindByNameAsync(string userName);

        Task<IdentityResult> AddUserToRoleAsync(User user, string roleName);
    }
}
