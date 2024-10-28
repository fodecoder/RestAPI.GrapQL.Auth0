using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Controllers
{
    [Route ( "[controller]" )]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtTokenConfig _jwtConfig;
        private readonly IMapper _mapper;

        public AuthenticationController( IMapper mapper , IOptions<JwtTokenConfig> config )
        {
            _mapper = mapper;
            _jwtConfig = config.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType ( StatusCodes.Status401Unauthorized )]
        [ProducesResponseType ( StatusCodes.Status200OK )]
        public IActionResult Authenticate()
        {
            // TODO check if user exisists in DB and use its actual role
            // credentials are ok, create a ClaimsIdentity for user
            var identity = new ClaimsIdentity ( JwtBearerDefaults.AuthenticationScheme );

            // add claim relative to user (useful for authorizations)
            identity.AddClaim ( new Claim ( ClaimTypes.Name , "User" ) );
            // insert here other claims (example: authenticated user's roles)
            identity.AddClaim ( new Claim ( ClaimTypes.Role , "Developer" ) );

            // associate identity to http request
            if (HttpContext != null)
            {
                HttpContext.User = new ClaimsPrincipal ( identity );
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
            var responseToken = _jwtSecurityTokenHandler.WriteToken ( token );

            // token will be created by middleware if user is authenticated
            return Ok ( responseToken );
        }

        [HttpGet ( "refresh" )]
        [Authorize ( Policy = JwtBearerDefaults.AuthenticationScheme )]
        [ProducesResponseType ( StatusCodes.Status200OK )]
        [ProducesResponseType ( StatusCodes.Status400BadRequest )]
        [ProducesResponseType ( StatusCodes.Status401Unauthorized )]
        public IActionResult RefreshToken()
        {
            // TODO refresh jwt token
            return Ok ();
        }
    }
}
