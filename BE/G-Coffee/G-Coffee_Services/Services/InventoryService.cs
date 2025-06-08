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
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryRepository _InventoryRepository;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unitOfWork, IInventoryRepository InventoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _InventoryRepository = InventoryRepository ?? throw new ArgumentNullException(nameof(InventoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<InventoryDTO> CreateInventoryAsync(InventoryDTO Inventory)
        {
            if (Inventory == null) throw new ArgumentNullException(nameof(Inventory));

            try
            {
                // Correctly map InventoryDTO to Inventory before passing to AddAsync
                var InventoryEntity = _mapper.Map<Inventory>(Inventory);
                

                await _InventoryRepository.AddAsync(InventoryEntity); // Fix: Pass the mapped Inventory entity
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<InventoryDTO>(InventoryEntity); // Fix: Return the mapped InventoryDTO
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Inventory", ex);
            }
        }

        public async Task DeleteInventoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Inventory ID is required");

            try
            {
                var Inventory = await _InventoryRepository.GetByIdAsync(Guid.Parse(id));
                if (Inventory == null) throw new Exception($"Inventory with ID {id} not found");

                _InventoryRepository.Remove(Inventory);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting Inventory with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<InventoryDTO>> GetAllInventorysAsync()
        {
            try
            {
                var Inventorys = await _InventoryRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<InventoryDTO>>(Inventorys);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all Inventorys", ex);
            }
        }

        public async Task<InventoryDTO> GetInventoryByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Inventory ID is required");

            try
            {
                var Inventory = await _InventoryRepository.GetByIdAsync(Guid.Parse(id));
                if (Inventory == null) throw new Exception($"Inventory with ID {id} not found");

                return _mapper.Map<InventoryDTO>(Inventory);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Inventory with ID {id}", ex);
            }
        }

        public async Task UpdateInventoryAsync(InventoryDTO InventoryDto)
        {
            if (InventoryDto == null) throw new ArgumentNullException(nameof(InventoryDto));

            try
            {
                var Inventory = await _InventoryRepository.GetByIdAsync(InventoryDto.InventoryId);
                if (Inventory == null) throw new Exception($"Inventory with ID {InventoryDto.InventoryId} not found");

                _mapper.Map(InventoryDto, Inventory);
                _InventoryRepository.Update(Inventory); // Removed 'await' since Update is a void method.
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating Inventory with ID {InventoryDto.InventoryId}", ex);
            }
        }
    }
}
