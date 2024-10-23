namespace RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model
{
    public class JwtTokenConfig
    {
        public string JwtTokenSecret { get; set; }
        public string JwtTokenIssuer { get; set; }
        public string JwtTokenAudience { get; set; }
        public string JwtTokenExpirationInMinutes { get; set; }
    }
}
