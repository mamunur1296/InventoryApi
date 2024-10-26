using InventoryApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        // Parse the token only if the route is not public and the method is Create/Update
        if (IsCreateOrUpdateMethod())
        {
            ParseToken();
        }
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
    // Check if the current HTTP method is for Create or Update
    private bool IsCreateOrUpdateMethod()
    {
        var method = _httpContextAccessor.HttpContext?.Request.Method.ToLower();

        // Only trigger token parsing for Create (POST) or Update (PUT/PATCH) operations
        return method == "post" || method == "put" || method == "patch";
    }
}
