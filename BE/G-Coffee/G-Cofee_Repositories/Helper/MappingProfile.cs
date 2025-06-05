using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Ánh xạ ProductDto -> Product
        CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.UnitOfMeasureId, opt => opt.MapFrom(src => src.UnitOfMeasureId))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

        // Ánh xạ Product -> ProductDto
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.UnitOfMeasureId, opt => opt.MapFrom(src => src.UnitOfMeasureId))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId));

        CreateMap<UserRegisterDTO, User>()
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
           .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => true));
        CreateMap<SupplierDTO, Supplier>()
          .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
          .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => true))
          .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        CreateMap<Supplier, SupplierDTO>(); // If reverse mapping is needed

    }
}