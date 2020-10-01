using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.DataAccessLayer;

namespace WingtipToys.BusinessLogicLayer
{
    public class WingtipProfile : Profile
    {
        public WingtipProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(m => m.CategoryName, map => map.MapFrom(p => p.Category.CategoryName))
                .ForMember(m => m.CategoryId, map => map.MapFrom(p => p.Category.Id));
            
            CreateMap<CartItem, CartItemDto>()
                .ForMember(m => m.ProductName, map => map.MapFrom(p => p.Product.ProductName))
                .ForMember(m => m.ImagePath, map => map.MapFrom(p => p.Product.ImagePath))
                .ForMember(m => m.UnitPrice, map => map.MapFrom(p => p.Product.UnitPrice));

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(m => m.ProductName, map => map.MapFrom(p => p.Product.ProductName))
                .ForMember(m => m.ImagePath, map => map.MapFrom(p => p.Product.ImagePath))
                .ForMember(m => m.UnitPrice, map => map.MapFrom(p => p.Product.UnitPrice));
            CreateMap<OrderDetailDto, OrderDetail>();
        }
    }
}
