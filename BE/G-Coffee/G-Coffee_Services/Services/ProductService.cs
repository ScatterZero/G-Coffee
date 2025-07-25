
using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Helper;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

// ... (các using giữ nguyên)

namespace G_Coffee_Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;
        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, IMapper mapper, ISupplierRepository supplierRepository, IHttpContextAccessor httpContextAccessor, IUnitOfMeasureRepository unitOfMeasureRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _supplierRepository = supplierRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfMeasureRepository = unitOfMeasureRepository;
        }

        public async Task<Product> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                if (productDto == null) throw new ArgumentNullException(nameof(productDto));
                if (string.IsNullOrEmpty(productDto.ProductName)) throw new ArgumentException("Product name is required");
                if (string.IsNullOrEmpty(productDto.UnitOfMeasureId)) throw new ArgumentException("Unit of measure is required");
                if (string.IsNullOrEmpty(productDto.ShortName)) throw new ArgumentException("Short name is required");
                if (productDto.SupplierId == null) throw new ArgumentException("Supplier ID is required");
                if (productDto.UnitPrice == null || productDto.UnitPrice < 0) throw new ArgumentException("Unit price must be non-negative");
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("Không thể xác định người dùng từ token.");
                bool supplierExists = await _supplierRepository.ExistsAsync(s => s.SupplierId == productDto.SupplierId);
                if (!supplierExists)
                    throw new KeyNotFoundException($"Supplier with ID {productDto.SupplierId} not found");


                var product = _mapper.Map<Product>(productDto);

                var math = new Caculate();
                do
                {
                    product.ProductID = math.GenerateEan13Barcode();
                } while (await _productRepository.ExistsAsync(p => p.ProductID == product.ProductID));

                product.CreatedDate = DateTime.UtcNow;
                product.UpdatedDate = DateTime.UtcNow;

                await _productRepository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to create product due to database error", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while creating product", ex);
            }
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentException("Product ID is required");

                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found");

                return _mapper.Map<Product>(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving product with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<Product>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all products", ex);
            }
        }

        public async Task<ReponseUpdateProductDto> UpdateProductAsync(ReponseUpdateProductDto product)
        {
            try
            {
                if (product == null) throw new ArgumentNullException(nameof(product));
                if (string.IsNullOrEmpty(product.ProductID)) throw new ArgumentException("Product ID is required");
                var existingProduct = await _productRepository.GetByIdAsync(product.ProductID);
                if (existingProduct == null) throw new KeyNotFoundException($"Product with ID {product.ProductID} not found");
                var supplierExists = await _supplierRepository.ExistsAsync(s => s.SupplierId == product.SupplierId);
                if (!supplierExists) throw new ArgumentException($"Supplier with ID {product.SupplierId} not found");
                var uomExists = await _unitOfMeasureRepository.ExistsAsync(u => u.UnitOfMeasureId == product.UnitOfMeasureId);
                if (!uomExists) throw new ArgumentException($"UnitOfMeasure with ID {product.UnitOfMeasureId} not found");
                // Update properties
                existingProduct.ProductName = product.ProductName;
                existingProduct.UnitOfMeasureId = product.UnitOfMeasureId;
                existingProduct.ShortName = product.ShortName;
                existingProduct.SupplierId = product.SupplierId;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.UpdatedDate = DateTime.UtcNow;
                existingProduct.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                existingProduct.IsDisabled = product.IsDisabled ?? false;
                _productRepository.Update(existingProduct);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ReponseUpdateProductDto>(existingProduct);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update product due to database error", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while updating product", ex);
            }


        }

        public async Task DeleteProductAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentException("Product ID is required");

                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) throw new KeyNotFoundException("Product not found");

                _productRepository.Remove(product);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to delete product due to database error", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while deleting product", ex);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsBySupplierIdAsync(string supplierId)
        {
            try
            {
                if (string.IsNullOrEmpty(supplierId)) throw new ArgumentException("Supplier ID is required");

                var products = await _productRepository.GetProductsBySupplierIdAsync(supplierId);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving products for supplier {supplierId}", ex);
            }
        }

        public async Task ImportProductsAsync(IEnumerable<ProductDto> productDtos)
        {
            try
            {
                if (productDtos == null) throw new ArgumentNullException(nameof(productDtos));
                if (!productDtos.Any()) throw new ArgumentException("No products to import");

                var math = new Caculate();
                var productsToAdd = new List<Product>();

                foreach (var dto in productDtos)
                {
                    if (string.IsNullOrEmpty(dto.ProductName)) throw new ArgumentException("Product name is required");
                    if (string.IsNullOrEmpty(dto.UnitOfMeasureId)) throw new ArgumentException("Unit of measure is required");
                    if (string.IsNullOrEmpty(dto.ShortName)) throw new ArgumentException("Short name is required");
                    if (dto.SupplierId == null) throw new ArgumentException("Supplier ID is required");
                    if (dto.UnitPrice == null || dto.UnitPrice < 0) throw new ArgumentException("Unit price must be non-negative");

                    var product = _mapper.Map<Product>(dto);
                    product.ProductID = math.GenerateEan13Barcode();
                    product.CreatedDate = DateTime.UtcNow;
                    product.UpdatedDate = DateTime.UtcNow;
                    productsToAdd.Add(product);
                }

                await _productRepository.AddRangeAsync(productsToAdd);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database error occurred while importing products", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while importing products", ex);
            }
        }
    }
}
