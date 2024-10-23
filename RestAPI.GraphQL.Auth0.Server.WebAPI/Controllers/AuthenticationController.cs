using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Controllers
{
    [Route ( "api/core/authentication" )]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;

        public AuthenticationController( IMapper mapper )
        {
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType ( StatusCodes.Status401Unauthorized )]
        [ProducesResponseType ( StatusCodes.Status200OK )]
        public IActionResult Authenticate()
        {
            //credentials are ok, create a ClaimsIdentity for user
            var identity = new ClaimsIdentity ( JwtBearerDefaults.AuthenticationScheme );

            //add claim relative to user (useful for authorizations)
            identity.AddClaim ( new Claim ( ClaimTypes.Name , "User" ) );
            // insert here other claims (example: authenticated user's roles)
            identity.AddClaim ( new Claim ( ClaimTypes.Role , "Developer" ) );

            // associate identity to http request
            if (HttpContext != null)
            {
                HttpContext.User = new ClaimsPrincipal ( identity );
            }

            // token will be created by middleware if user is authenticated
            return Ok ();
        }

        [HttpGet]
        [Route ( "refresh" )]
        [Authorize]
        [ProducesResponseType ( StatusCodes.Status200OK )]
        [ProducesResponseType ( StatusCodes.Status400BadRequest )]
        [ProducesResponseType ( StatusCodes.Status401Unauthorized )]
        public IActionResult RefreshToken()
        {
            // token will be created by middleware if user is authenticated
            return Ok ();
        }
    }
}
