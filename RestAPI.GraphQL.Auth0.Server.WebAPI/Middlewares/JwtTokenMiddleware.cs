using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Middlewares
{
    public sealed class JwtTokenMiddleware : IMiddleware
    {
        private readonly JwtTokenConfig _jwtConfig;

        public JwtTokenMiddleware( IOptions<JwtTokenConfig> config )
        {
            _jwtConfig = config.Value;
        }

        public async Task InvokeAsync( HttpContext context , RequestDelegate next )
        {
            // Get the token from the Authorization header
            var token = context.Request.Headers["Authorization"].ToString ().Replace ( "Bearer " , "" );

            if (!token.IsNullOrEmpty ())
            {
                try
                {
                    // Verify the token using the JwtSecurityTokenHandlerWrapper
                    var claimsPrincipal = ValidateJwtToken ( token );

                    // Extract the user ID from the token
                    var userId = claimsPrincipal.FindFirst ( ClaimTypes.NameIdentifier )?.Value;

                    // Store the user ID in the HttpContext items for later use
                    context.Items["UserId"] = userId;

                    // You can also do the for same other key which you have in JWT token.
                }
                catch (Exception)
                {
                    // If the token is invalid, throw an exception
                    //context.Result = new UnauthorizedResult ();
                }
            }

            if (context != null)
            {
                context.Response.OnStarting ( () =>
                {
                    var identity = context.User.Identity as ClaimsIdentity;

                    if (identity.IsAuthenticated)
                    {
                        // create token for next request
                        var token = GenerateJwtToken ( identity );
                        context.Response.Headers.Add ( "X-Token" , token );
                    }
                    return Task.CompletedTask;
                } );
            }

            // Continue processing the request
            await next.Invoke ( context );
        }

        // Generate a JWT token based on user ID and role
        public string GenerateJwtToken( ClaimsIdentity identity )
        {
            if (identity is null)
            {
                throw new ArgumentNullException ( nameof ( identity ) );
            }

            // Retrieve the JWT secret from environment variables and encode it
            var key = Encoding.UTF8.GetBytes ( _jwtConfig.JwtTokenSecret! );

            if (!double.TryParse ( _jwtConfig.JwtTokenExpirationInMinutes , out var expirationMinutes ))
            {
                expirationMinutes = 40;
            }

            // Describe the token settings
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtConfig.JwtTokenIssuer ,
                Audience = _jwtConfig.JwtTokenAudience ,
                Subject = identity ,
                Expires = DateTime.UtcNow.AddMinutes ( expirationMinutes ) ,
                SigningCredentials = new SigningCredentials ( new SymmetricSecurityKey ( key ) , SecurityAlgorithms.HmacSha256Signature )
            };

            // Create a JWT security token
            var _jwtSecurityTokenHandler = new JwtSecurityTokenHandler ();
            var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken ( tokenDescriptor );

            // Write the token as a string and return it
            return _jwtSecurityTokenHandler.WriteToken ( token );
        }

        // Validate a JWT token
        public ClaimsPrincipal ValidateJwtToken( string token )
        {
            // Retrieve the JWT secret from environment variables and encode it
            var key = Encoding.UTF8.GetBytes ( _jwtConfig.JwtTokenSecret! );

            try
            {
                // Create a token handler and validate the token
                var tokenHandler = new JwtSecurityTokenHandler ();
                var claimsPrincipal = tokenHandler.ValidateToken ( token , new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true ,
                    ValidIssuer = _jwtConfig.JwtTokenIssuer ,
                    ValidAudience = _jwtConfig.JwtTokenAudience ,
                    IssuerSigningKey = new SymmetricSecurityKey ( key )
                } , out SecurityToken validatedToken );

                // Return the claims principal
                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                // Handle token expiration
                throw new ApplicationException ( "Token has expired." );
            }
        }
    }
}
