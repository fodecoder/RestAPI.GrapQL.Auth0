using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.Services.Extension
{
    public static class SwaggerExtension
    {
        public static void AddSwagger( this IServiceCollection serviceCollection )
        {
            serviceCollection.AddSwaggerGen ( c =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication" ,
                    Description = "Enter JWT Bearer token **_only_**" ,
                    In = ParameterLocation.Header ,
                    Type = SecuritySchemeType.Http ,
                    Scheme = "bearer" , // must be lower case
                    BearerFormat = "JWT" ,
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme ,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.CustomOperationIds ( e => e.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor ?
                    controllerActionDescriptor.MethodInfo.Name :
                    e.ActionDescriptor.AttributeRouteInfo?.Name );
                c.AddSecurityDefinition ( securityScheme.Reference.Id , securityScheme );
                c.AddSecurityRequirement ( new OpenApiSecurityRequirement
                  {
                    {securityScheme, Array.Empty<string>()}
                  } );
                //c.OperationFilter<SwaggerEtagHeaderFilter> ();
                //c.OperationFilter<SwaggerVersionHeaderFilter> ();
            } );
            //serviceCollection.AddTransient<IConfigureOptions<SwaggerGenOptions> , SwaggerInfoOptions> ();
        }
    }
}
