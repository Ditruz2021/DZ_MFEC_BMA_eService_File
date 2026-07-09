using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_starter.Utils
{
    public class AuthorizationUtils
    {
        private readonly string _secretKey;
        private readonly ILogger<AuthorizationUtils> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationUtils(
            IConfiguration configuration,
            ILogger<AuthorizationUtils> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _secretKey = configuration["Jwt:SecretKey"] ?? "";
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        public string GenerateToken(int id, string name, string username, int roleId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", id.ToString()),
                new Claim("name", name),
                new Claim("username", username),
                new Claim("roleId", roleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "dotnet_starter",
                audience: "dotnet_starter",
                claims: claims,
                // expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        public ClaimsPrincipal? VerifyToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "dotnet_starter",
                    ValidAudience = "dotnet_starter",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public string? GetIdFromToken()
        {
            var userId = _httpContextAccessor.HttpContext?.Items["userId"]?.ToString();
            return userId;
        }
        public string? GetRoleIdFromToken()
        {
            var roleId = _httpContextAccessor.HttpContext?.Items["roleId"]?.ToString();
            return roleId;
        }
        public string? GetNameFromToken()
        {
            var name = _httpContextAccessor.HttpContext?.Items["name"]?.ToString();
            return name;
        }
        public string? GetUsernameFromToken()
        {
            var username = _httpContextAccessor.HttpContext?.Items["username"]?.ToString();
            return username;
        }
    }
}
