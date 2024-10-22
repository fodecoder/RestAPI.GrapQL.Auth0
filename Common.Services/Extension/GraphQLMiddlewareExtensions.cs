using Common.Services.Middlewares;
using Common.Services.Options;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Services.Extension
{
    public static class GraphQLMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphQL( this IApplicationBuilder builder )
        {
            return builder.UseMiddleware<GraphQLMiddleware> ();
        }

        public static IServiceCollection AddGraphQLExtension<T>( this IServiceCollection services , Action<GraphQLOptions> action ) where T : ObjectGraphType
        {
            services.AddGraphQL ( builder => builder
                .AddAutoSchema<T> ()
                .AddSystemTextJson () );

            return services.Configure ( action );
        }
    }
}
