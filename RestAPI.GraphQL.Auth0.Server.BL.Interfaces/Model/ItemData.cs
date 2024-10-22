using GraphQL.Types;

namespace RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model
{
    public class ItemData : ObjectGraphType<Item>
    {
        public ItemData()
        {
            Field ( x => x.Id ).Description ( "The Id of the Item." );
            Field ( x => x.Name ).Description ( "The name of the Item." );
            Field ( x => x.Description ).Description ( "The description of the Item." );
            Field ( x => x.Type ).Description ( "The type of the Item." );
        }
    }
}
