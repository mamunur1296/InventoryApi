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

    // Check if the current route is public (no token required)
    private bool IsPublicRoute()
    {
        var path = _httpContextAccessor.HttpContext?.Request.Path.ToString().ToLower();

        // Define public URL patterns, allowing for dynamic product IDs (GUID)
        var publicPaths = new List<string>
        {
            "/api/product/all",
            "/api/product/get/"
        };

        // Check static public paths
        if (publicPaths.Any(p => path.Contains(p)))
        {
            return true;
        }

        // Define dynamic path pattern for GUIDs
        var dynamicPathPatterns = new List<string>
        {
            @"^/api/product/get/[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$"
        };

        // Check if the path matches any of the dynamic patterns (like GUID paths)
        return dynamicPathPatterns.Any(pattern => System.Text.RegularExpressions.Regex.IsMatch(path, pattern));
    }

    // Check if the current HTTP method is for Create or Update
    private bool IsCreateOrUpdateMethod()
    {
        var method = _httpContextAccessor.HttpContext?.Request.Method.ToLower();

        // Only trigger token parsing for Create (POST) or Update (PUT/PATCH) operations
        return method == "post" || method == "put" || method == "patch";
    }
}
