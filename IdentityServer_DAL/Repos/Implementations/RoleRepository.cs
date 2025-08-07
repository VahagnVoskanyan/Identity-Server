using IdentityServer_DAL.Repos.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer_DAL.Repos.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        protected RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager) => _roleManager = roleManager;

        public async Task<IdentityResult> CreateRoleAsync(IdentityRole identityRole)
        {
            return await _roleManager.CreateAsync(identityRole);
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole?> GetRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<IdentityRole?> FindByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }
    }
}
