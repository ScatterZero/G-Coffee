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
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(ITransactionRepository transactionRepository, IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }



        public async Task ImportReceipt(TransactionDTO transaction)
        {
            if (transaction.TransactionType != "Import")
                throw new ArgumentException("Invalid transaction type. Expected 'Import'.");

            if (transaction.TotalQuantity <= 0 || transaction.TotalAmount <= 0)
                throw new ArgumentException("Quantity and amount must be greater than zero.");

            if (string.IsNullOrEmpty(transaction.SupplierId))
                throw new ArgumentNullException(nameof(transaction.SupplierId));
            if (transaction.TransactionDetails == null || !transaction.TransactionDetails.Any())
                throw new ArgumentException("Transaction must have at least one detail.");

            // Map TransactionDTO to Transaction
            var transactionEntity = _mapper.Map<Transaction>(transaction);
            transactionEntity.TransactionId = Guid.NewGuid();
            transactionEntity.CreatedDate = DateTime.Now;
            transactionEntity.Status = "Completed";

            // Save Transaction first
            await _transactionRepository.AddAsync(transactionEntity);

            // Process TransactionDetails
            foreach (var detailDto in transaction.TransactionDetails)
            {
                if (string.IsNullOrEmpty(detailDto.ProductId) || string.IsNullOrEmpty(detailDto.WarehouseId))
                    throw new ArgumentNullException("ProductId or WarehouseId cannot be null or empty.");

                // Fetch and update inventory
                var inventory =
                    await _inventoryRepository.GetByProductAndWarehouseAsync(detailDto.ProductId,
                        detailDto.WarehouseId);
                if (inventory == null)
                {
                    inventory = new Inventory
                    {
                        InventoryId = Guid.NewGuid(),
                        ProductId = detailDto.ProductId,
                        WarehouseId = detailDto.WarehouseId,
                        Quantity = 0,
                        LastUpdated = DateTime.Now
                    };
                }

                inventory.Quantity += detailDto.Quantity;
                inventory.LastUpdated = DateTime.Now;

                if (!await _inventoryRepository.ExistsAsync(i =>
                        i.ProductId == detailDto.ProductId && i.WarehouseId == detailDto.WarehouseId))
                    await _inventoryRepository.AddAsync(inventory);
                else
                    _inventoryRepository.Update(inventory);

                // Map and add TransactionDetail
                var detailEntity = _mapper.Map<TransactionDetail>(detailDto);
                detailEntity.TransactionDetailId = Guid.NewGuid();
                detailEntity.TransactionId = transactionEntity.TransactionId;
                detailEntity.CreatedDate = DateTime.Now;
                transactionEntity.TransactionDetails.Add(detailEntity);
            }

            // Save all changes
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TransactionDTO> ExportReceipt(TransactionDTO transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction.TransactionType != "Export")
                throw new ArgumentException("Invalid transaction type. Expected 'Export'.", nameof(transaction.TransactionType));

            if (transaction.TotalQuantity <= 0 || transaction.TotalAmount <= 0)
                throw new ArgumentException("Total quantity and amount must be greater than zero.", nameof(transaction));

            // if (string.IsNullOrEmpty(transaction.CustomerId))
            //     throw new ArgumentNullException(nameof(transaction.CustomerId), "CustomerId cannot be null or empty for export transactions.");

            if (transaction.TransactionDetails == null || !transaction.TransactionDetails.Any())
                throw new ArgumentException("Transaction must contain at least one detail.", nameof(transaction.TransactionDetails));

            // Map TransactionDTO to Transaction
            var transactionEntity = _mapper.Map<Transaction>(transaction) ?? throw new InvalidOperationException("Failed to map TransactionDTO to Transaction entity.");
            transactionEntity.TransactionId = Guid.NewGuid();
            transactionEntity.CreatedDate = DateTime.Now;
            transactionEntity.Status = "Completed";
            transactionEntity.TransactionDetails = new List<TransactionDetail>();

            // Save Transaction first
            await _transactionRepository.AddAsync(transactionEntity);

            // Process TransactionDetails
            foreach (var detailDto in transaction.TransactionDetails)
            {
                if (string.IsNullOrEmpty(detailDto.ProductId) || string.IsNullOrEmpty(detailDto.WarehouseId))
                    throw new ArgumentNullException("ProductId or WarehouseId cannot be null or empty in transaction details.");

                if (detailDto.Quantity <= 0)
                    throw new ArgumentException($"Quantity must be greater than zero for ProductId: {detailDto.ProductId}.", nameof(detailDto.Quantity));

                // Fetch inventory
                var inventory = await _inventoryRepository.GetByProductAndWarehouseAsync(detailDto.ProductId, detailDto.WarehouseId);
                if (inventory == null)
                    throw new InvalidOperationException($"No inventory found for ProductId: {detailDto.ProductId} in WarehouseId: {detailDto.WarehouseId}.");

                // Check if sufficient quantity is available
                if (inventory.Quantity < detailDto.Quantity)
                    throw new InvalidOperationException($"Insufficient inventory for ProductId: {detailDto.ProductId} in WarehouseId: {detailDto.WarehouseId}. Available: {inventory.Quantity}, Requested: {detailDto.Quantity}.");

                // Update inventory
                inventory.Quantity -= detailDto.Quantity;
                inventory.LastUpdated = DateTime.Now;
                _inventoryRepository.Update(inventory);

                // Map and add TransactionDetail
                var detailEntity = _mapper.Map<TransactionDetail>(detailDto) ?? throw new InvalidOperationException("Failed to map TransactionDetailDTO to TransactionDetail entity.");
                detailEntity.TransactionDetailId = Guid.NewGuid();
                detailEntity.TransactionId = transactionEntity.TransactionId;
                detailEntity.CreatedDate = DateTime.Now;
                transactionEntity.TransactionDetails.Add(detailEntity);
            }

            // Save all changes
            await _unitOfWork.SaveChangesAsync();

            // Map back to DTO for return
            return _mapper.Map<TransactionDTO>(transactionEntity);
         }
        }
      
    
}
