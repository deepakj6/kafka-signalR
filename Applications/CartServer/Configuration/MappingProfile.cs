using AutoMapper;
using CartServer.Contracts.Requests;
using CartServer.Contracts.Responses;
using CartServer.Models;

namespace CartServer.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartRequest, Cart>();
            CreateMap<CartItemRequest, CartItem>();
            CreateMap<Cart, CartResponse>();
            CreateMap<CartItem, CartItemResponse>();
        }
    }
}
