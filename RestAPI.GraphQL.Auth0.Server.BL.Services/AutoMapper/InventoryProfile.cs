using AutoMapper;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Model;
using DB = Common.DB.Model;

namespace RestAPI.GraphQL.Auth0.Server.BL.Services.AutoMapper
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<DB.Item , Item> ();
        }
    }
}
