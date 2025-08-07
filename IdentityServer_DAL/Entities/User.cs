using Microsoft.AspNetCore.Identity;

namespace IdentityServer_DAL.Entities
{
    public class User : IdentityUser
    {
        public string OrganizationName { get; set; } = null!;
    }
}
