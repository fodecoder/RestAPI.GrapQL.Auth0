using HotChocolate.Authorization;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Service;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Queries
{
    [Authorize]
    public class InventoryQuery
    {
        [GraphQLDeprecated ( "Use the `itemList` field instead" )]
        public async Task<IEnumerable<Item>> GetItems( [Service] IItemService _itemService , string name , int limit )
        {
            return await _itemService.GetItemsAsync ( name , limit );
        }

        [GraphQLName ( "PaginatedItems" )]
        [UsePaging ( IncludeTotalCount = true )]
        public async Task<IEnumerable<Item>> GetItemsList( [Service] IItemService _itemService , string name )
        {
            return await _itemService.GetItemsAsync ( name , int.MaxValue );
        }
    }
}
