using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, ISupplierRepository supplierRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplier)
        {
            if (supplier == null) throw new ArgumentException("Supplier data cannot be null");

            var supplierEntity = _mapper.Map<Supplier>(supplier);
            supplierEntity.CreatedDate = DateTime.UtcNow;
            supplierEntity.UpdatedDate = DateTime.UtcNow;

            await _supplierRepository.AddAsync(supplierEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SupplierDTO>(supplierEntity);
        }

        public async Task DeleteSupplierAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Supplier ID is required");

            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
                throw new KeyNotFoundException($"Supplier with ID {id} not found");

            _supplierRepository.Remove(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }

        public async Task<SupplierDTO> GetSupplierByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Supplier ID is required");

            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
                throw new KeyNotFoundException($"Supplier with ID {id} not found");

            return _mapper.Map<SupplierDTO>(supplier);
        }

        public async Task UpdateSupplierAsync(SupplierDTO supplierDto)
        {
            if (supplierDto == null) throw new ArgumentException("Supplier data cannot be null");

            var supplier = await _supplierRepository.GetByIdAsync(supplierDto.SupplierId);
            if (supplier == null)
                throw new KeyNotFoundException($"Supplier with ID {supplierDto.SupplierId} not found");

            _mapper.Map(supplierDto, supplier);
            _supplierRepository.Update(supplier);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
