using dotnet_starter.Presenters;
using dotnet_starter.Utils;

namespace dotnet_starter.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthorizationUtils _authorization;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(
            RequestDelegate next,
            AuthorizationUtils authorization,
            ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _authorization = authorization;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if
            (
                context.Request.Path.StartsWithSegments("/web/sign-in") ||
                context.Request.Path.StartsWithSegments("/web/auth/user-generate") ||
                context.Request.Path.StartsWithSegments("/storage")
            )
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers.Authorization.ToString();
            var token = authHeader?.Split(' ') is { Length: > 1 } parts ? parts[^1] : null;

            if (string.IsNullOrEmpty(token))
            {
                await UnauthorizedResponse(context, "Token is missing");
                return;
            }

            var userClaims = _authorization.VerifyToken(token);
            if (userClaims == null)
            {
                _logger.LogWarning("Token is expired.");
                await UnauthorizedResponse(context, "Token expired");
                return;
            }


            var claimsDict = userClaims.Claims.ToDictionary(c => c.Type, c => c.Value);

            if (claimsDict.TryGetValue("userId", out var userId)) context.Items["userId"] = userId;
            if (claimsDict.TryGetValue("roleId", out var roleId)) context.Items["roleId"] = roleId;
            if (claimsDict.TryGetValue("username", out var username)) context.Items["username"] = username;
            if (claimsDict.TryGetValue("name", out var name)) context.Items["name"] = name;
            await _next(context);
        }

        private static async Task UnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new ResponseError<string>
            {
                Status = false,
                RequestId = context.TraceIdentifier.Replace(":", ""),
                ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ErrorType = "authorization",
                Message = message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
