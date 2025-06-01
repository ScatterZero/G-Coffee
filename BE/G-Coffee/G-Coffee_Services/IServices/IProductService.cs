using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;

namespace G_Coffee_Services.IServices
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task<ProductDto> GetProductByIdAsync(string id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task UpdateProductAsync(ProductDto productDto);
        Task DeleteProductAsync(string id);
        Task<IEnumerable<ProductDto>> GetProductsBySupplierIdAsync(string supplierId);
    }
}
