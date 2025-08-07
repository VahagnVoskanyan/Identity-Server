using AutoMapper;
using IdentityServer.DTOs.User;
using IdentityServer_DAL.Entities;

namespace IdentityServer.Profiles
{
    /// <summary>
    /// For Model Automated mapping
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Source -> Target

            CreateMap<User, UserResponseDto>();
        }
    }
}
