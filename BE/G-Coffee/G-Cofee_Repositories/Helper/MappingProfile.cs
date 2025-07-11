﻿using AutoMapper;
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
           .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Ánh xạ Product -> ProductDto
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.UnitOfMeasureId, opt => opt.MapFrom(src => src.UnitOfMeasureId))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId));
        CreateMap<UserLoginDTO, User>()
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        CreateMap<UserRegisterDTO, User>()
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
           .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => true));
        CreateMap<User, UserRegisterDTO>();
        CreateMap<UserUpdateDTO, User>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());// ✅ Không map UserId;
        CreateMap<SupplierDTO, Supplier>()
          .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
          .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => true))
          .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        CreateMap<Supplier, SupplierDTO>(); // If reverse mapping is needed
        CreateMap<UnitOfMeasureDTO, UnitsOfMeasure>()
      .ForMember(dest => dest.UnitOfMeasureId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        CreateMap<UnitsOfMeasure, UnitOfMeasureDTO>();
        CreateMap<InventoryDTO, Inventory>()
            .ForMember(dest => dest.InventoryId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        CreateMap<Inventory, InventoryDTO>().ReverseMap();
        // Warehouse mapping
            CreateMap<WarehouseDTO, Warehouse>()
                .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
        CreateMap<Warehouse, WarehouseDTO>()
     .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId.ToString()));
        // Transaction 
        CreateMap<TransactionDTO, Transaction>()
      .ForMember(dest => dest.TransactionDetails, opt => opt.MapFrom(src => src.TransactionDetails));
        CreateMap<TransactionDetailDTO, TransactionDetail>();
        // TransactionDetail mapping
        CreateMap<TransactionDetailDTO, TransactionDetail>()
            .ForMember(dest => dest.TransactionDetailId, opt => opt.MapFrom(src => src.TransactionDetailId))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
        CreateMap<TransactionDetail, TransactionDetailDTO>();
        // Order
        CreateMap<OrderDTO, Order>();
        CreateMap<Order, OrderDTO>();
        // payment mapping
        CreateMap<PaymentWrapperResponse, Payment>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
        CreateMap<Payment, PaymentWrapperResponse>();
        // Combopackage mapping
        CreateMap<Payment, PaymentDTO>().ReverseMap();
   





    }
}