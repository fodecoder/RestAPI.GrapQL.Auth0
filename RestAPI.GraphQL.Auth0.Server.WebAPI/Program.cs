using Common.DB.Extension;
using Common.Services.Extension;
using Common.Services.Schemas;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Repository;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Service;
using RestAPI.GraphQL.Auth0.Server.BL.Services.AutoMapper;
using RestAPI.GraphQL.Auth0.Server.BL.Services.Context;
using RestAPI.GraphQL.Auth0.Server.BL.Services.Queries;
using RestAPI.GraphQL.Auth0.Server.BL.Services.Repository;
using RestAPI.GraphQL.Auth0.Server.BL.Services.Services;
using RestAPI.GraphQL.Auth0.Server.WebAPI.Helpers;
using RestAPI.GraphQL.Auth0.Server.WebAPI.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder ( args );

// Add services to the container.

builder.Services.AddControllers ();

builder.Services.AddEndpointsApiExplorer ();

// Add Repositories
builder.Services.AddScoped<IItemRepository , ItemRepository> ();

// Add Services
builder.Services.AddScoped<IItemService , ItemService> ();

// Add AutoMapper
builder.Services.AddAutoMapper ( typeof ( InventoryProfile ) );

// Add Db Context
builder.Services.AddDatabase<InventoryDBContext> ();

// Add GraphQL 
builder.Services.AddSingleton<IDocumentExecuter , DocumentExecuter> ();
builder.Services.AddSingleton<IGraphQLSerializer , GraphQLSerializer> ();
builder.Services.AddTransient<InventoryQuery> ();
builder.Services.AddScoped<ISchema , GraphQLSchema<InventoryQuery>> ( services => new GraphQLSchema<InventoryQuery> ( new SelfActivatingServiceProvider ( services ) ) );
builder.Services.AddGraphQLExtension<InventoryQuery> ( options =>
{
    options.EndPoint = "/graphql";
} );

// Add Authorisation and Authentication
builder.Services.AddTransient<JwtTokenMiddleware> ();
builder.Services.Configure<JwtTokenConfig> ( builder.Configuration.GetSection ( nameof ( JwtTokenConfig ) ) );
builder.Services
    .AddAuthentication ( options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    } )
    .AddJwtBearer ( options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true ,
            ValidateAudience = true ,
            ValidateIssuerSigningKey = true ,

            // Same Issuer, Audience and secret key
            // used in JwtTokenMiddleware
            ValidIssuer = builder.Configuration.GetSection ( nameof ( JwtTokenConfig ) )["JwtTokenIssuer"] ,
            ValidAudience = builder.Configuration.GetSection ( nameof ( JwtTokenConfig ) )["JwtTokenAudience"] ,
            IssuerSigningKey = new SymmetricSecurityKey ( Encoding.UTF8.GetBytes ( builder.Configuration.GetSection ( nameof ( JwtTokenConfig ) )["JwtTokenSecret"]! ) ) ,
            ClockSkew = TimeSpan.Zero
        };
    } );

// Configure the default authorization policy
builder.Services.AddAuthorization ( options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder ()
            .AddAuthenticationSchemes ( JwtBearerDefaults.AuthenticationScheme )
            .RequireAuthenticatedUser ()
            .Build ();
} );

// Add the global scope for JwtSecurityTokenHandlerWrapper.
builder.Services.AddScoped<JwtSecurityTokenHandlerWrapper> ();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger ();

var app = builder.Build ();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ())
{
    app.UseSwagger ();
    app.UseSwaggerUI ();
}

app.UseHttpsRedirection ();

app.UseMiddleware<JwtTokenMiddleware> ();

app.UseAuthentication ();
app.UseRouting ();
app.UseAuthorization ();

app.MapControllers ();

// GraphQL
app.UseGraphQLGraphiQL ();
app.UseGraphQL<ISchema> ();

app.Run ();
