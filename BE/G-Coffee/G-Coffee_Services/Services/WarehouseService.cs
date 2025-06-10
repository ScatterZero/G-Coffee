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
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWarehouseRepository _WarehouseRepository;
        private readonly IMapper _mapper;

        public WarehouseService(IUnitOfWork unitOfWork, IWarehouseRepository WarehouseRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _WarehouseRepository = WarehouseRepository ?? throw new ArgumentNullException(nameof(WarehouseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO Warehouse)
        {
            if (Warehouse == null) throw new ArgumentNullException(nameof(Warehouse));

            try
            {
                // Correctly map WarehouseDTO to Warehouse before passing to AddAsync
                var WarehouseEntity = _mapper.Map<Warehouse>(Warehouse);


                await _WarehouseRepository.AddAsync(WarehouseEntity); // Fix: Pass the mapped Warehouse entity
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<WarehouseDTO>(WarehouseEntity); // Fix: Return the mapped WarehouseDTO
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Warehouse", ex);
            }
        }

        public async Task DeleteWarehouseAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Warehouse ID is required");

            try
            {
                var Warehouse = await _WarehouseRepository.GetByIdAsync(id);
                if (Warehouse == null) throw new Exception($"Warehouse with ID {id} not found");

                _WarehouseRepository.Remove(Warehouse);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting Warehouse with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<WarehouseDTO>> GetAllWarehousesAsync()
        {
            try
            {
                var Warehouses = await _WarehouseRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<WarehouseDTO>>(Warehouses);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all Warehouses", ex);
            }
        }

        public async Task<WarehouseDTO> GetWarehouseByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Warehouse ID is required");

            try
            {
                var Warehouse = await _WarehouseRepository.GetByIdAsync(id);
                if (Warehouse == null) throw new Exception($"Warehouse with ID {id} not found");

                return _mapper.Map<WarehouseDTO>(Warehouse);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Warehouse with ID {id}", ex);
            }
        }

        public async Task UpdateWarehouseAsync(WarehouseDTO WarehouseDto)
        {
            if (WarehouseDto == null) throw new ArgumentNullException(nameof(WarehouseDto));

            try
            {
                var Warehouse = await _WarehouseRepository.GetByIdAsync(WarehouseDto.WarehouseId);
                if (Warehouse == null) throw new Exception($"Warehouse with ID {WarehouseDto.WarehouseId} not found");

                _mapper.Map(WarehouseDto, Warehouse);
                _WarehouseRepository.Update(Warehouse); // Removed 'await' since Update is a void method.
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating Warehouse with ID {WarehouseDto.WarehouseId}", ex);
            }
        }
    }
}
