using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using G_Cofee_Repositories.Models;

namespace G_Cofee_Repositories.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> ExistsAsync(Expression<Func<Product, bool>> value);
        Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(string supplierId);
    }
}
