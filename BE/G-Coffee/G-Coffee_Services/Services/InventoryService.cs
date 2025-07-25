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
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public InventoryService(IUnitOfWork unitOfWork, IInventoryRepository inventoryRepository, IMapper mapper, IProductRepository productRepository, IWarehouseRepository warehouseRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Inventory> CreateInventoryAsync(InventoryDTO inventoryDto)
        {
            if (inventoryDto == null)
                throw new ArgumentException("Inventory DTO cannot be null");
            var existingProduct = await _productRepository.GetByIdAsync(inventoryDto.ProductID);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {inventoryDto.ProductID} not found");
            var existingWarehouse = await _warehouseRepository.GetByIdAsync(inventoryDto.WarehouseId);
            if (existingWarehouse == null)
                throw new KeyNotFoundException($"Warehouse with ID {inventoryDto.WarehouseId} not found");

            var entity = _mapper.Map<Inventory>(inventoryDto);
            entity.InventoryId = Guid.NewGuid();
            entity.LastUpdated = DateTime.UtcNow;
            entity.Min = inventoryDto.Min != 0 ? inventoryDto.Min : 0;
            entity.Max = inventoryDto.Max != 0 ? inventoryDto.Max : 0;
            entity.WarehouseId = inventoryDto.WarehouseId;
            entity.ProductID = inventoryDto.ProductID;


            await _inventoryRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;

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

        public async Task<IEnumerable<Inventory>> GetAllInventorysAsync()
        {
            var inventories = await _inventoryRepository.GetAllInventory();
            if (inventories == null || !inventories.Any())
                throw new KeyNotFoundException("No inventories found");
            return _mapper.Map<IEnumerable<Inventory>>(inventories);
        }

        public async Task<Inventory> GetInventoryByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Inventory ID is required");

            if (!Guid.TryParse(id, out var guidId))
                throw new ArgumentException("Invalid Inventory ID format");

            var inventory = await _inventoryRepository.GetByInventoryIdAsync(guidId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory with ID {id} not found");
            return _mapper.Map<Inventory>(inventory);

        }

        public async Task<InventoryUpdateDTO> UpdateInventoryAsync(InventoryUpdateDTO inventory)
        {
            if (inventory == null)
                throw new ArgumentException("Inventory cannot be null");
            if (string.IsNullOrWhiteSpace(inventory.InventoryId.ToString()))
                throw new ArgumentException("Inventory ID is required");
            var existingInventory = await _inventoryRepository.GetByIdAsync(inventory.InventoryId);
            if (existingInventory == null)
                throw new KeyNotFoundException($"Inventory with ID {inventory.InventoryId} not found");
            existingInventory.WarehouseId = inventory.WarehouseId ?? existingInventory.WarehouseId;
            existingInventory.ProductID = inventory.ProductID ?? existingInventory.ProductID;
            existingInventory.Quantity = inventory.Quantity ?? existingInventory.Quantity;
            existingInventory.LastUpdated = inventory.LastUpdated ?? existingInventory.LastUpdated;
            existingInventory.Min = inventory.Min != 0 ? inventory.Min : existingInventory.Min;
            existingInventory.Max = inventory.Max != 0 ? inventory.Max : existingInventory.Max;
            _mapper.Map(inventory, existingInventory);
            _inventoryRepository.Update(existingInventory);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<InventoryUpdateDTO>(existingInventory);

        }
    }
}
