using Common.Services.Extension;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Service;

namespace RestAPI.GraphQL.Auth0.Server.BL.Interfaces.ProxyServices
{
    public class ProxyItemService : IItemService
    {
        private readonly HttpClient _httpClient;

        public ProxyItemService( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        public async Task CreateItemAsync( Item element )
        {
            var url = Uri.EscapeDataString ( $"item" );
            await _httpClient.PostAsync<Item> ( url , element );
        }

        public async Task DeleteItemAsync( Guid id )
        {
            var url = Uri.EscapeDataString ( $"item/{id}" );
            await _httpClient.DeleteAsync<Item> ( url );
        }

        public Task<IEnumerable<Item>> GetItemsAsync( string name , int limit )
        {
            throw new NotImplementedException ();
        }

        public async Task<Item> ReadItemAsync( Guid id )
        {
            var url = Uri.EscapeDataString ( $"item/{id}" );
            return await _httpClient.GetAsync<Item> ( url );
        }

        public async Task UpdateItemAsync( Item element )
        {
            var url = Uri.EscapeDataString ( $"item" );
            await _httpClient.PutAsync<Item> ( url , element );
        }
    }
}
