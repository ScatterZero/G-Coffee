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
        Task<Product> CreateProductAsync(ProductDto productDto);
        Task<Product> GetProductByIdAsync(string id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<Product> UpdateProductAsync(Product productid);
        Task DeleteProductAsync(string id);
        Task<IEnumerable<ProductDto>> GetProductsBySupplierIdAsync(string supplierId);
    }
}
