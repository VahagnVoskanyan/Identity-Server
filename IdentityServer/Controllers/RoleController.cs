using IdentityServer_DAL.Repos.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepo;

        public RoleController(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<IdentityResult>> CreateRole([FromBody] string roleName)
        {
            var identityRole = new IdentityRole
            {
                Name = roleName
            };

            var result = await _roleRepo.CreateRoleAsync(identityRole);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllRoles()
        {
            var roles = await _roleRepo.GetAllRolesAsync();
            return Ok(roles.Select(r => r.Name));
        }
    }
}
