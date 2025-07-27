using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace G_Coffee_Services.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WarehouseService(IUnitOfWork unitOfWork, IWarehouseRepository warehouseRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO warehouseDto)
        {
            var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");

            if (warehouseDto == null)
                throw new ArgumentNullException(nameof(warehouseDto));

            var warehouseEntity = _mapper.Map<Warehouse>(warehouseDto);
            warehouseEntity.TenantID = tenantId ?? throw new InvalidOperationException("Tenant ID is required");
            await _warehouseRepository.AddAsync(warehouseEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<WarehouseDTO>(warehouseEntity);
        }

        public async Task DeleteWarehouseAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Warehouse ID is required", nameof(id));

            var warehouse = await _warehouseRepository.GetByIdAsync(id);
            if (warehouse == null)
                throw new KeyNotFoundException($"Warehouse with ID {id} not found");

            _warehouseRepository.Remove(warehouse);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<WarehouseDTO>> GetAllWarehousesAsync()
        {
            var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");
            var warehouses = await _warehouseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<WarehouseDTO>>(warehouses);
        }

        public async Task<WarehouseDTO> GetWarehouseByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Warehouse ID is required", nameof(id));

            var warehouse = await _warehouseRepository.GetByIdAsync(id);
            if (warehouse == null)
                throw new KeyNotFoundException($"Warehouse with ID {id} not found");

            return _mapper.Map<WarehouseDTO>(warehouse);
        }

        public async Task UpdateWarehouseAsync(WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null)
                throw new ArgumentNullException(nameof(warehouseDto));

            var warehouse = await _warehouseRepository.GetByIdAsync(warehouseDto.WarehouseId);
            if (warehouse == null)
                throw new KeyNotFoundException($"Warehouse with ID {warehouseDto.WarehouseId} not found");

            _mapper.Map(warehouseDto, warehouse);
            _warehouseRepository.Update(warehouse);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
