
using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Helper;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ... (các using giữ nguyên)

namespace G_Coffee_Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                if (productDto == null) throw new ArgumentNullException(nameof(productDto));
                if (string.IsNullOrEmpty(productDto.ProductName)) throw new ArgumentException("Product name is required");
                if (string.IsNullOrEmpty(productDto.UnitOfMeasureId)) throw new ArgumentException("Unit of measure is required");
                if (string.IsNullOrEmpty(productDto.ShortName)) throw new ArgumentException("Short name is required");
                if (productDto.SupplierId == null) throw new ArgumentException("Supplier ID is required");
                if (productDto.UnitPrice == null || productDto.UnitPrice < 0) throw new ArgumentException("Unit price must be non-negative");

                var math = new Caculate();
                do
                {
                    productDto.ProductID = math.GenerateEan13Barcode();
                } while (await _productRepository.ExistsAsync(p => p.ProductID == productDto.ProductID));

                var product = _mapper.Map<Product>(productDto);
                product.CreatedDate = DateTime.UtcNow;
                product.UpdatedDate = DateTime.UtcNow;

                await _productRepository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<ProductDto>(product);
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

        public async Task<ProductDto> GetProductByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentException("Product ID is required");

                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found");

                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving product with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all products", ex);
            }
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            try
            {
                if (productDto == null) throw new ArgumentNullException(nameof(productDto));
                if (string.IsNullOrEmpty(productDto.ProductID)) throw new ArgumentException("Product ID is required");
                if (string.IsNullOrEmpty(productDto.ProductName)) throw new ArgumentException("Product name is required");
                if (string.IsNullOrEmpty(productDto.UnitOfMeasureId)) throw new ArgumentException("Unit of measure is required");
                if (productDto.UnitPrice == null || productDto.UnitPrice < 0) throw new ArgumentException("Unit price must be non-negative");

                var product = await _productRepository.GetByIdAsync(productDto.ProductID);
                if (product == null) throw new KeyNotFoundException("Product not found");

                _mapper.Map(productDto, product);
                product.UpdatedDate = DateTime.UtcNow;

                _productRepository.Update(product);
                await _unitOfWork.SaveChangesAsync();
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

                    do
                    {
                        dto.ProductID = math.GenerateEan13Barcode();
                    } while (await _productRepository.ExistsAsync(p => p.ProductID == dto.ProductID));

                    var product = _mapper.Map<Product>(dto);
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
