using Microsoft.AspNetCore.Authentication.JwtBearer;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.WebAPI.ApiKey;
using System.Security.Claims;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Middlewares
{
    public sealed class ApiKeyValidationMiddleware : IMiddleware
    {
        private readonly ApiKeyValidation _apiKeyValidation;

        public ApiKeyValidationMiddleware( ApiKeyValidation apiKeyValidation )
        {
            _apiKeyValidation = apiKeyValidation;
        }

        public async Task InvokeAsync( HttpContext context , RequestDelegate next )
        {
            // Get the token from the Authorization header
            var apiKey = context.Request.Headers[Constants.ApiKeyHeaderName];

            bool isValid = _apiKeyValidation.IsValidApiKey ( apiKey );

            if (context != null)
            {
                context.Response.OnStarting ( () =>
                {
                    if (isValid)
                    {
                        // TODO define role depending on the API Key used
                        var identity = new ClaimsIdentity ( JwtBearerDefaults.AuthenticationScheme );

                        // add claim relative to user (useful for authorizations)
                        identity.AddClaim ( new Claim ( ClaimTypes.Name , "User" ) );
                        // insert here other claims (example: authenticated user's roles)
                        identity.AddClaim ( new Claim ( ClaimTypes.Role , "Developer" ) );

                        // associate identity to http request
                        context.User = new ClaimsPrincipal ( identity );
                    }
                    return Task.CompletedTask;
                } );
            }

            // Continue processing the request
            await next.Invoke ( context );
        }
    }
}
