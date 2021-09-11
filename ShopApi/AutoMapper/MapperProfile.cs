using AutoMapper;
using ShopApi.Dto.AddressDTO;
using ShopApi.Dto.OrderLines;
using ShopApi.Dto.Orders;
using ShopApi.Dto.Products;
using ShopApi.Dto.ShopDTO;
using ShopApi.Entities;
using ShopApi.Enums;
using ShopApi.Models;

namespace ShopApi.AutoMapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, CsvModel>()
                .ForMember(s => s.IsConfirmed, w =>
                    w.MapFrom(v => v.IsConfirmed == true ? "Potwierdzono" : "Brak Potwierdzenia"))
                .ForMember(s => s.CreatedDate, w =>
                       w.MapFrom(v => v.CreatedDate.ToString("F")))
                .ForMember(s => s.DateOfOrderExecution, w => w.MapFrom(v =>
                         v.DateOfOrderExecution == null 
                             ? "Nie Wykonano" 
                             : v.DateOfOrderExecution.Value.ToString("d")));

            CreateMap<CreateOrderLineDto, OrderLine>();
            CreateMap<OrderLine, OrderLine>();

            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product,UpdateProductDto>().ReverseMap();

            CreateMap<Shop, ShopDto>()
                .ForMember(s=>s.City,q=>q.MapFrom(r=>r.Address.City));
            CreateMap<CreateShopDto, Shop>()
                .ForMember(s => s.Address, d => d.MapFrom(q => new Address
                {
                    City=q.City.ToUpper(),
                    Street=q.Street.ToUpper(),
                    ZipCode=q.ZipCode.ToUpper()
                }));
            CreateMap<UpdateShopDto, Shop>();
            CreateMap<Shop, ShopDetailsDto>()
                .ForMember(s => s.City, c => c.MapFrom(q => q.Address.City))
                .ForMember(s => s.Street, c => c.MapFrom(q => q.Address.Street))
                .ForMember(s => s.ZipCode, c => c.MapFrom(q => q.Address.ZipCode));

            CreateMap<Address, AddressDto>();
        }
    }
}
