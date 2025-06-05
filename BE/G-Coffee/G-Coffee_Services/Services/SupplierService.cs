using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (supplier == null) throw new ArgumentNullException(nameof(supplier));

            try
            {
                // Correctly map SupplierDTO to Supplier before passing to AddAsync
                var supplierEntity = _mapper.Map<Supplier>(supplier);
                supplierEntity.CreatedDate = DateTime.UtcNow;
                supplierEntity.UpdatedDate = DateTime.UtcNow;

                await _supplierRepository.AddAsync(supplierEntity); // Fix: Pass the mapped Supplier entity
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<SupplierDTO>(supplierEntity); // Fix: Return the mapped SupplierDTO
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating supplier", ex);
            }
        }

        public async Task DeleteSupplierAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Supplier ID is required");

            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(id);
                if (supplier == null) throw new Exception($"Supplier with ID {id} not found");

                _supplierRepository.Remove(supplier);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting supplier with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            try
            {
                var suppliers = await _supplierRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all suppliers", ex);
            }
        }

        public async Task<SupplierDTO> GetSupplierByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Supplier ID is required");

            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(id);
                if (supplier == null) throw new Exception($"Supplier with ID {id} not found");

                return _mapper.Map<SupplierDTO>(supplier);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving supplier with ID {id}", ex);
            }
        }

        public async Task UpdateSupplierAsync(SupplierDTO supplierDto)
        {
            if (supplierDto == null) throw new ArgumentNullException(nameof(supplierDto));

            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(supplierDto.SupplierId);
                if (supplier == null) throw new Exception($"Supplier with ID {supplierDto.SupplierId} not found");

                _mapper.Map(supplierDto, supplier);
                _supplierRepository.Update(supplier); // Removed 'await' since Update is a void method.
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating supplier with ID {supplierDto.SupplierId}", ex);
            }
        }
    }
}
