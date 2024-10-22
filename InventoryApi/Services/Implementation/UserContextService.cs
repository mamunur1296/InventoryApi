using InventoryApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        // Parse the token and store claims in HttpContext Items
        ParseToken();
    }

    public string UserName => _httpContextAccessor.HttpContext?.Items["UserName"]?.ToString();
    public string UserId => _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
    public string UserRole => _httpContextAccessor.HttpContext?.Items["UserRole"]?.ToString();

    private void ParseToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            return; // No token found, skip
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
        {
            return; // Invalid token
        }

        // Extract claims from the token
        var userName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userRole = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

        // Store claims in HttpContext.Items
        _httpContextAccessor.HttpContext.Items["UserName"] = userName;
        _httpContextAccessor.HttpContext.Items["UserId"] = userId;
        _httpContextAccessor.HttpContext.Items["UserRole"] = userRole;
    }
}

