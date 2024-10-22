using Common.DB.Model;

namespace RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Repository
{
    public interface IItemRepository
    {
        // CRUD operations
        Task CreateDBItemAsync( Item element );
        Task<Item> ReadDBItemAsync( Guid id );
        Task UpdateDBItemAsync( Item element );
        Task DeleteDBItemAsync( Guid id );

        // List
        Task<IEnumerable<Item>> GetDBItemListAsync( string name , int limit );
    }
}
