using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Service;

namespace RestAPI.GraphQL.Auth0.Server.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route ( "[controller]" )]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        private readonly ILogger<ItemController> _logger;

        public ItemController( ILogger<ItemController> logger , IItemService itemService )
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpPost]
        public async Task CreateItem( [FromBody] Item item )
        {
            _logger.Log ( LogLevel.Debug , $"Created Item: {item}" );
            await _itemService.CreateItemAsync ( item );
        }

        [HttpGet ( "{id}" )]
        public Task<Item> ReadItem( [FromRoute] Guid id )
        {
            _logger.Log ( LogLevel.Debug , $"Requested Item: {id}" );
            return _itemService.ReadItemAsync ( id );
        }

        [HttpPut]
        public async Task UpdateItem( [FromBody] Item item )
        {
            _logger.Log ( LogLevel.Debug , $"Updated Item: {item}" );
            await _itemService.UpdateItemAsync ( item );
        }

        [HttpDelete ( "{id}" )]
        public async Task DeleteItem( Guid id )
        {
            _logger.Log ( LogLevel.Debug , $"Deleted Item: {id}" );
            await _itemService.DeleteItemAsync ( id );
        }
    }
}
