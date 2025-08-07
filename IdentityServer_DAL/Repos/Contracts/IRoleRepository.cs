using Microsoft.AspNetCore.Identity;

namespace IdentityServer_DAL.Repos.Contracts
{
    public interface IRoleRepository
    {
        Task<IdentityResult> CreateRoleAsync(IdentityRole identityRole);

        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();

        Task<IdentityRole?> GetRoleByIdAsync(string id);

        Task<IdentityRole?> FindByNameAsync(string roleName);
    }
}
