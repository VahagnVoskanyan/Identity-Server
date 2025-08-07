using System.ComponentModel.DataAnnotations;

namespace IdentityServer.DTOs.User
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string OrganizationName { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string RoleName { get; set; } = null!;
    }
}
