using AutoMapper;
using IdentityServer.DTOs.User;
using IdentityServer_DAL.Entities;
using IdentityServer_DAL.Repos.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<IdentityResult>> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                OrganizationName = dto.OrganizationName,
            };

            var result = await _userRepo.CreateUserAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            result = await _userRepo.AddUserToRoleAsync(user, dto.RoleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();

            var userDtos = _mapper.Map<IEnumerable<UserResponseDto>>(users);

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(string id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserResponseDto>(user);

            return Ok(userDto);
        }
    }
}
