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
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unitOfWork, IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<InventoryDTO> CreateInventoryAsync(InventoryDTO inventoryDto)
        {
            if (inventoryDto == null)
                throw new ArgumentException("Inventory DTO cannot be null");

            var entity = _mapper.Map<Inventory>(inventoryDto);
            await _inventoryRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InventoryDTO>(entity);
        }

        public async Task DeleteInventoryAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Inventory ID is required");

            if (!Guid.TryParse(id, out var guidId))
                throw new ArgumentException("Invalid Inventory ID format");

            var inventory = await _inventoryRepository.GetByIdAsync(guidId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory with ID {id} not found");

            _inventoryRepository.Remove(inventory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryDTO>> GetAllInventorysAsync()
        {
            var inventories = await _inventoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InventoryDTO>>(inventories);
        }

        public async Task<InventoryDTO> GetInventoryByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Inventory ID is required");

            if (!Guid.TryParse(id, out var guidId))
                throw new ArgumentException("Invalid Inventory ID format");

            var inventory = await _inventoryRepository.GetByIdAsync(guidId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory with ID {id} not found");

            return _mapper.Map<InventoryDTO>(inventory);
        }

        public async Task UpdateInventoryAsync(InventoryDTO inventoryDto)
        {
            if (inventoryDto == null)
                throw new ArgumentException("Inventory DTO cannot be null");

            var inventory = await _inventoryRepository.GetByIdAsync(inventoryDto.InventoryId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory with ID {inventoryDto.InventoryId} not found");

            _mapper.Map(inventoryDto, inventory);
            _inventoryRepository.Update(inventory);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
