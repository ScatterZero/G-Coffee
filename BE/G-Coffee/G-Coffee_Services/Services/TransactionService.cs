using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace G_Coffee_Services.Services
{
    //public class TransactionService : ITransactionService
    //{
    //    private readonly ITransactionRepository _transactionRepository;
    //    private readonly IInventoryRepository _inventoryRepository;
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;
    //    private readonly IHttpContextAccessor _httpContextAccessor;

    //    public TransactionService(
    //        ITransactionRepository transactionRepository,
    //        IInventoryRepository inventoryRepository,
    //        IUnitOfWork unitOfWork,
    //        IMapper mapper,
    //        IHttpContextAccessor httpContextAccessor)
    //    {
    //        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
    //        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
    //        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    //        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    public async Task ImportReceipt(TransactionDTO transaction)
    //    {
    //        var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");

    //        if (transaction == null)
    //            throw new ArgumentNullException(nameof(transaction));

    //        if (transaction.TransactionType != "Import")
    //            throw new ArgumentException("Invalid transaction type. Expected 'Import'.");

    //        if (transaction.TotalQuantity <= 0 || transaction.TotalAmount <= 0)
    //            throw new ArgumentException("Quantity and amount must be greater than zero.");

    //        if (string.IsNullOrEmpty(transaction.SupplierId))
    //            throw new ArgumentNullException(nameof(transaction.SupplierId));


    //        var random = new Random();
    //        var transactionEntity = _mapper.Map<Transaction>(transaction);
    //        transactionEntity.TenantID = tenantId ?? throw new InvalidOperationException("Tenant ID is required");
    //        transactionEntity.TransactionId = transactionEntity.TransactionId = "IP" + random.Next(100000, 999999) + ""; 
    //        transactionEntity.CreatedDate = DateTime.Now;
    //        transactionEntity.Status = "Completed";

    //        await _transactionRepository.AddAsync(transactionEntity);

    //        //foreach (var detailDto in transaction.TransactionDetails)
    //        //{
    //        //    if (string.IsNullOrEmpty(detailDto.ProductId) || string.IsNullOrEmpty(detailDto.WarehouseId))
    //        //        throw new ArgumentNullException("ProductId or WarehouseId cannot be null or empty.");

    //        //    var inventory = await _inventoryRepository.GetByProductAndWarehouseAsync(
    //        //        detailDto.ProductId, detailDto.WarehouseId
    //        //    );

    //        //    if (inventory == null)
    //        //    {
    //        //        inventory = new Inventory
    //        //        {
    //        //            InventoryId = Guid.NewGuid(),
    //        //            ProductID = detailDto.ProductId,
    //        //            WarehouseId = detailDto.WarehouseId,
    //        //            Quantity = 0,
    //        //            LastUpdated = DateTime.Now
    //        //        };
    //        //    }

    //        //    inventory.Quantity += detailDto.Quantity;
    //        //    inventory.LastUpdated = DateTime.Now;

    //        //    var exists = await _inventoryRepository.ExistsAsync(i =>
    //        //        i.ProductID == detailDto.ProductId &&
    //        //        i.WarehouseId == detailDto.WarehouseId);

    //        //    if (!exists)
    //        //        await _inventoryRepository.AddAsync(inventory);
    //        //    else
    //        //        _inventoryRepository.Update(inventory);

    //        //    var detailEntity = _mapper.Map<TransactionDetail>(detailDto);
    //        //    detailEntity.TransactionDetailId = Guid.NewGuid();
    //        //    detailEntity.TransactionId = transactionEntity.TransactionId;
    //        //    detailEntity.CreatedDate = DateTime.Now;

    //        //    transactionEntity.TransactionDetails.Add(detailEntity);
    //        //}

    //        await _unitOfWork.SaveChangesAsync();
    //    }

    //    public async Task<TransactionDTO> ExportReceipt(TransactionDTO transaction)
    //    {
    //        var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");
    //        if (transaction == null)
    //            throw new ArgumentNullException(nameof(transaction));

    //        if (transaction.TransactionType != "Export")
    //            throw new ArgumentException("Invalid transaction type. Expected 'Export'.");

    //        if (transaction.TotalQuantity <= 0 || transaction.TotalAmount <= 0)
    //            throw new ArgumentException("Total quantity and amount must be greater than zero.");


    //        var random = new Random();
    //        var transactionEntity = _mapper.Map<Transaction>(transaction);
    //        transactionEntity.TenantID = tenantId ?? throw new InvalidOperationException("Tenant ID is required");
    //        transactionEntity.TransactionId = "EP" + random.Next(100000, 999999) + "";
    //        transactionEntity.CreatedDate = DateTime.Now;
    //        transactionEntity.Status = "Completed";
    //        transactionEntity.TransactionDetails = new List<TransactionDetail>();

    //        await _transactionRepository.AddAsync(transactionEntity);

    //        //foreach (var detailDto in transaction.TransactionDetails)
    //        //{
    //        //    if (string.IsNullOrEmpty(detailDto.ProductId) || string.IsNullOrEmpty(detailDto.WarehouseId))
    //        //        throw new ArgumentNullException("ProductId or WarehouseId cannot be null or empty.");

    //        //    if (detailDto.Quantity <= 0)
    //        //        throw new ArgumentException($"Quantity must be greater than zero for ProductId: {detailDto.ProductId}.");

    //        //    var inventory = await _inventoryRepository.GetByProductAndWarehouseAsync(detailDto.ProductId, detailDto.WarehouseId);
    //        //    if (inventory == null)
    //        //        throw new InvalidOperationException($"No inventory found for ProductId: {detailDto.ProductId} in Warehouse: {detailDto.WarehouseId}");

    //        //    if (inventory.Quantity < detailDto.Quantity)
    //        //        throw new InvalidOperationException($"Insufficient inventory for ProductId: {detailDto.ProductId} in Warehouse: {detailDto.WarehouseId}. Available: {inventory.Quantity}, Requested: {detailDto.Quantity}");

    //        //    inventory.Quantity -= detailDto.Quantity;
    //        //    inventory.LastUpdated = DateTime.Now;
    //        //    _inventoryRepository.Update(inventory);

    //        //    var detailEntity = _mapper.Map<TransactionDetail>(detailDto);
    //        //    detailEntity.TransactionDetailId = Guid.NewGuid();
    //        //    detailEntity.TransactionId = transactionEntity.TransactionId;
    //        //    detailEntity.CreatedDate = DateTime.Now;

    //        //    transactionEntity.TransactionDetails.Add(detailEntity);
    //        //}

    //        await _unitOfWork.SaveChangesAsync();

    //        return _mapper.Map<TransactionDTO>(transactionEntity);
    //    }

    //    public Task<Transaction> GetTransactionByIdAsync(string transactionId)
    //    {
    //        if (string.IsNullOrEmpty(transactionId))
    //            throw new ArgumentNullException(nameof(transactionId));
    //        return _transactionRepository.GetTransactionByIdAsync(transactionId);
    //    }

    //    public Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    //    {
    //        var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");
    //        return _transactionRepository.GetAllTransactionsAsync(tenantId);
    //    }
    //}

        public class TransactionService : ITransactionService
        {
            private readonly ITransactionRepository _transactionRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public TransactionService(
                ITransactionRepository transactionRepository,
                IUnitOfWork unitOfWork,
                IMapper mapper,
                IHttpContextAccessor httpContextAccessor)
            {
                _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _httpContextAccessor = httpContextAccessor;
            }

            private async Task<Transaction> CreateTransactionAsync(TransactionDTO transaction, string transactionType, string prefix)
            {
                var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");
                if (transaction == null)
                    throw new ArgumentNullException(nameof(transaction));

                if (transaction.TransactionType != transactionType)
                    throw new ArgumentException($"Invalid transaction type. Expected '{transactionType}'.");

                if (transaction.TotalQuantity <= 0 || transaction.TotalAmount <= 0)
                    throw new ArgumentException("Quantity and amount must be greater than zero.");

                if (transactionType == "Import" && string.IsNullOrEmpty(transaction.SupplierId))
                    throw new ArgumentNullException(nameof(transaction.SupplierId));

                var random = new Random();
                var transactionEntity = _mapper.Map<Transaction>(transaction);
                transactionEntity.TenantID = tenantId ?? throw new InvalidOperationException("Tenant ID is required");
                transactionEntity.TransactionId = prefix + random.Next(100000, 999999);
                transactionEntity.CreatedDate = DateTime.Now;
                transactionEntity.TransactionDetails = new List<TransactionDetail>();

                await _transactionRepository.AddAsync(transactionEntity);
                return transactionEntity;
            }

            public async Task<Transaction> ImportReceipt(TransactionDTO transaction)
            {
                var transactionEntity = await CreateTransactionAsync(transaction, "Import", "IP");
                await _unitOfWork.SaveChangesAsync();
                return transactionEntity;
        }

            public async Task<Transaction> ExportReceipt(TransactionDTO transaction)
            {
                var transactionEntity = await CreateTransactionAsync(transaction, "Export", "EP");
                await _unitOfWork.SaveChangesAsync();
                return transactionEntity;
            }

            public Task<Transaction> GetTransactionByIdAsync(string transactionId)
            {
                if (string.IsNullOrEmpty(transactionId))
                    throw new ArgumentNullException(nameof(transactionId));
                return _transactionRepository.GetTransactionByIdAsync(transactionId);
            }

            public Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
            {
                var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");
                return _transactionRepository.GetAllTransactionsAsync(tenantId);
            }
        }
    }

