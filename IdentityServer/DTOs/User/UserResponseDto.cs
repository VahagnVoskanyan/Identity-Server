namespace IdentityServer.DTOs.User
{
    public class UserResponseDto
    {
        public string UserName { get; set; } = null!;

        public string OrganizationName { get; set; } = null!;

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }
    }
}
