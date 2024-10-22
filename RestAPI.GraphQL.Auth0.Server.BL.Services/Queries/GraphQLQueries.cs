using GraphQL;
using GraphQL.Types;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Service;

namespace RestAPI.GraphQL.Auth0.Server.BL.Services.Queries
{
    public class InventoryQuery : ObjectGraphType
    {
        public InventoryQuery( IItemService itemService )
        {
            Field<ListGraphType<ItemData>> ( "items" )
                .Description ( "Returns a collection of items." )
                .Arguments ( new QueryArguments ( new QueryArgument<StringGraphType> { Name = "name" } , new QueryArgument<IntGraphType> { Name = "limit" } ) )
                .ResolveAsync ( async context =>
                {
                    var retValue = await itemService.GetItemsAsync ( context.GetArgument<string> ( "name" ) , context.GetArgument<int> ( "limit" ) );
                    return retValue;
                } );
        }
    }
}
