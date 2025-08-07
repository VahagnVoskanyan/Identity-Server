using IdentityServer.DTOs.Auth;
using IdentityServer.Services.JWT;
using IdentityServer_DAL.Entities;
using IdentityServer_DAL.Repos.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository userRepo,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ITokenService tokenService)
        {
            _userRepo = userRepo;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepo.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
                return Unauthorized("Invalid username or password.");

            // creating the necessary claims
            List<Claim> authClaims = [
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.NameIdentifier, user.OrganizationName)];

            var accessToken = _tokenService.GenerateAccessToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Saving the refresh token
            await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", refreshToken);

            return Ok(new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Token/Refresh")]
        public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] TokenDto dto) 
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null)
                return BadRequest("Invalid access token");

            var username = principal.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return BadRequest("Invalid principal");

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return Unauthorized("User not found");

            // Get the saved refresh token from AspNetUserTokens
            var savedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshToken");

            if (savedRefreshToken != dto.RefreshToken)
                return Unauthorized("Invalid refresh token");

            // creating the necessary claims
            List<Claim> authClaims = [
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.NameIdentifier, user.OrganizationName)];

            var accessToken = _tokenService.GenerateAccessToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Saving the refresh token
            await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", refreshToken);

            return Ok(new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
