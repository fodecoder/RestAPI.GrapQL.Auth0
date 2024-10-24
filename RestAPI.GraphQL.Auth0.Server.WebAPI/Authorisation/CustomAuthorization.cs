using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.WebAPI.ApiKey;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Authorisation
{
    [AttributeUsage ( AttributeTargets.Class | AttributeTargets.Method )]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization( AuthorizationFilterContext filterContext )
        {
            if (filterContext != null)
            {
                var apiKeyValidation = filterContext.HttpContext.RequestServices
                    .GetService<ApiKeyValidation> ();
                var jwtConfig = filterContext.HttpContext.RequestServices
                    .GetService<IConfiguration> ()
                    ?.GetSection ( nameof ( JwtTokenConfig ) )
                    .Get<JwtTokenConfig> ();

                // get the authorization header
                Microsoft.Extensions.Primitives.StringValues authTokens;
                filterContext.HttpContext.Request.Headers.TryGetValue ( "Authorization" , out authTokens );

                var _token = authTokens.FirstOrDefault ()?.ToString ().Replace ( "Bearer " , "" );

                if (_token != null)
                {
                    string authToken = _token;
                    if (authToken != null)
                    {
                        if (IsValidToken ( jwtConfig , authToken ))
                        {
                            filterContext.HttpContext.Response.Headers.Add ( "Authorization" , authToken );
                            filterContext.HttpContext.Response.Headers.Add ( "AuthStatus" , "Authorized" );

                            filterContext.HttpContext.Response.Headers.Add ( "storeAccessiblity" , "Authorized" );

                            return;
                        }
                        else
                        {
                            filterContext.HttpContext.Response.Headers.Add ( "Authorization" , authToken );
                            filterContext.HttpContext.Response.Headers.Add ( "AuthStatus" , "NotAuthorized" );

                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature> ().ReasonPhrase = "Not Authorized";
                            filterContext.Result = new JsonResult ( "NotAuthorized" )
                            {
                                Value = new
                                {
                                    Status = "Error" ,
                                    Message = "Invalid Token"
                                } ,
                            };
                        }
                    }
                }
                else
                {
                    // if the request header doesn't contain the authorization header, try to get the API-Key.
                    Microsoft.Extensions.Primitives.StringValues apikey;
                    var key = filterContext.HttpContext.Request.Headers.TryGetValue ( Constants.ApiKeyHeaderName , out apikey );
                    var keyvalue = apikey.FirstOrDefault ();

                    // if the API-Key value is not null. validate the API-Key.
                    if (keyvalue != null && apiKeyValidation.IsValidApiKey ( keyvalue ))
                    {
                        filterContext.HttpContext.Response.Headers.Add ( "ApiKey" , keyvalue );
                        filterContext.HttpContext.Response.Headers.Add ( "AuthStatus" , "Authorized" );

                        filterContext.HttpContext.Response.Headers.Add ( "storeAccessiblity" , "Authorized" );

                        return;
                    }

                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature> ().ReasonPhrase = "Please Provide authToken";
                    filterContext.Result = new JsonResult ( "Please Provide auth Token or API Key" )
                    {
                        Value = new
                        {
                            Status = "Error" ,
                            Message = "Please Provide auth Token"
                        } ,
                    };
                }
            }
        }

        public bool IsValidToken( JwtTokenConfig _jwtConfig , string authToken )
        {
            // Retrieve the JWT secret from environment variables and encode it
            var key = Encoding.UTF8.GetBytes ( _jwtConfig.JwtTokenSecret! );

            try
            {
                // Create a token handler and validate the token
                var tokenHandler = new JwtSecurityTokenHandler ();
                var claimsPrincipal = tokenHandler.ValidateToken ( authToken , new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true ,
                    ValidIssuer = _jwtConfig.JwtTokenIssuer ,
                    ValidAudience = _jwtConfig.JwtTokenAudience ,
                    IssuerSigningKey = new SymmetricSecurityKey ( key )
                } , out SecurityToken validatedToken );

                // Return the claims principal
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                // Handle token expiration
                throw new ApplicationException ( "Token has expired." );
            }
            catch
            {
                return false;
            }
        }
    }
}
