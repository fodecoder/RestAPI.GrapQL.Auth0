using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.ApiKey
{
    public class ApiKeyValidation
    {
        private readonly IConfiguration _configuration;

        public ApiKeyValidation( IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public bool IsValidApiKey( string userApiKey )
        {
            if (string.IsNullOrWhiteSpace ( userApiKey ))
                return false;

            string? apiKey = _configuration.GetValue<string> ( Constants.ApiKeyName );

            if (apiKey == null || apiKey != userApiKey)
                return false;
            return true;
        }
    }
}
