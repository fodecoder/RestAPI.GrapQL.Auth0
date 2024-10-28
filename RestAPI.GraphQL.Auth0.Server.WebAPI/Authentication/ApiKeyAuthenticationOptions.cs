using Microsoft.AspNetCore.Authentication;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Authentication
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = Constants.ApiKeySchemeName;
        public string ApiKeyHeaderName { get; set; } = Constants.ApiKeyHeaderName;
    }
}
