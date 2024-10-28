using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using RestAPI.GraphQL.Auth0.Server.WebAPI.ApiKey;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public ApiKeyAuthenticationHandler( IOptionsMonitor<ApiKeyAuthenticationOptions> options , ILoggerFactory logger , UrlEncoder encoder , ISystemClock clock )
            : base ( options , logger , encoder , clock )
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.TryGetValue ( Options.ApiKeyHeaderName , out var apiKey ))
            {
                var apiKeyValidation = Context.RequestServices
                    .GetService<ApiKeyValidation> ();

                var _apiKey = apiKey.FirstOrDefault ();

                // if the API-Key value is not null. validate the API-Key.
                if (_apiKey != null && apiKeyValidation.IsValidApiKey ( _apiKey ))
                {
                    Context.Response.Headers.Add ( "ApiKey" , _apiKey );
                    Context.Response.Headers.Add ( "AuthStatus" , "Authorized" );

                    var claims = new List<Claim> ()
                    {
                        // TODO update role depending on the Api Key used
                        new Claim(ClaimTypes.Role, "Developer")
                    };

                    var claimsIdentity = new ClaimsIdentity
                        ( claims , Scheme.Name );
                    var claimsPrincipal = new ClaimsPrincipal
                        ( claimsIdentity );

                    return Task.FromResult ( AuthenticateResult.Success
                        ( new AuthenticationTicket ( claimsPrincipal ,
                        Scheme.Name ) ) );
                }
                else
                {
                    Context.Response.Headers.Add ( "ApiKey" , _apiKey );
                    Context.Response.Headers.Add ( "AuthStatus" , "NotAuthorized" );

                    Context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    Context.Response.HttpContext.Features.Get<IHttpResponseFeature> ().ReasonPhrase = "Not Authorized";
                }
            }

            return Task.FromResult ( AuthenticateResult.Fail ( $"Missing header: {Options.ApiKeyHeaderName}" ) );
        }
    }
}
