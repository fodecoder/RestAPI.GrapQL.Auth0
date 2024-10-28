using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Authentication
{
    public class AuthenticationInterceptor : DefaultHttpRequestInterceptor
    {
        public override async ValueTask OnCreateAsync( HttpContext context , IRequestExecutor requestExecutor , OperationRequestBuilder requestBuilder , CancellationToken cancellationToken )
        {
            var result = await context.AuthenticateAsync ( Constants.ApiKeySchemeName );
            if (!result.Succeeded)
            {
                result = await context.AuthenticateAsync ( JwtBearerDefaults.AuthenticationScheme ); // find a better way to do this. currently the claims only get populated if we successfully authenticate, so we need to do it twice. this takes the default scheme
            }

            // not an else here because we reassign the value of result. Just looks confusing
            if (result.Succeeded)
            {
                context.User = result.Principal;
            }
            await base.OnCreateAsync ( context , requestExecutor , requestBuilder , cancellationToken );
        }
    }
}
