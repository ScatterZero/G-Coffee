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

        public Task<TransactionDTO> ExportReceipt(TransactionDTO entity)
        {
            throw new NotImplementedException();
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
                var inventory = await _inventoryRepository.GetByProductAndWarehouseAsync(detailDto.ProductId, detailDto.WarehouseId);
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

                if (!await _inventoryRepository.ExistsAsync(i => i.ProductId == detailDto.ProductId && i.WarehouseId == detailDto.WarehouseId))
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
    }
}
