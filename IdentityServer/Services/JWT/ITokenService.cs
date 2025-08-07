using System.Security.Claims;

namespace IdentityServer.Services.JWT
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken);
    }
}
