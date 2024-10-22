using Common.DB.Model;
using Microsoft.EntityFrameworkCore;

namespace RestAPI.GraphQL.Auth0.Server.BL.Services.Context
{
    public class InventoryDBContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public InventoryDBContext( DbContextOptions<InventoryDBContext> options )
        : base ( options )
        {
        }
    }
}
