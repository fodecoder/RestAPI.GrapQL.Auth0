using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Service;

namespace RestAPI.GraphQL.Auth0.Client.ConsoleApp
{
    public class Tester
    {
        private readonly IItemService _itemService;

        public Tester( IItemService itemService )
        {
            _itemService = itemService;
        }

        public async Task Run()
        {
            Console.WriteLine ( "Testin APIs..." );

            Console.WriteLine ( "Test create..." );
            await _itemService.CreateItemAsync (
                new Item ()
                {
                    Name = "Test - " + new Random ().Next ( 0 , 10000 ) ,
                    Description = "Description - " + new Random ().Next ( 0 , 10000 ) ,
                    Type = "Type - " + new Random ().Next ( 0 , 10 )
                } );

            Console.WriteLine ( "Test Read..." );
            await _itemService.ReadItemAsync ( Guid.Parse ( "D71E936A-AABB-4A9D-B26D-0474802E8406" ) );

            Console.WriteLine ( "Test Update..." );
            await _itemService.UpdateItemAsync (
                new Item ()
                {
                    Id = Guid.Parse ( "D71E936A-AABB-4A9D-B26D-0474802E8406" ) ,
                    Name = "Test - " + new Random ().Next ( 0 , 10000 ) ,
                    Description = "Description - " + new Random ().Next ( 0 , 10000 ) ,
                    Type = "Type - " + new Random ().Next ( 0 , 10 )
                } );
        }
    }
}
