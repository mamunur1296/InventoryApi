using InventoryApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace InventoryApi.Services.Implementation
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expiryMinutes;

        public TokenGenerator(string key, string issuer, string audience, string expiryMinutes)
        {
            _key = key;
            _issuer = issuer;
            _audience = audience;
            _expiryMinutes = expiryMinutes;
        }

        public string GenerateJWTToken((string userId, string userName, string FName, string LName, string email, string img, IList<string> roles) userDetails)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var (userId, userName, FName, LName, email, img, roles) = userDetails;
            var imageUrl = string.IsNullOrEmpty(img) ? "https://via.placeholder.com/60" : img;
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Jti should be a unique identifier, not userId
                new Claim(ClaimTypes.Name, userName),
                new Claim("UserId", userId),
                new Claim("FName", FName),
                new Claim("LName", LName),
                new Claim("Email", email),
                new Claim("Img", imageUrl)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_expiryMinutes)),
                signingCredentials: signingCredentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }



    }

}
