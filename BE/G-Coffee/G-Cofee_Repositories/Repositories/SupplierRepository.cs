using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        private readonly GcoffeeDbContext _context;
        public SupplierRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context;
        }

        // Phương thức kiểm tra xem nhà cung cấp có tồn tại theo điều kiện nhất định hay không
        public async Task<bool> ExistsAsync(Expression<Func<Supplier, bool>> predicate)
        {
            // Corrected to use the Suppliers DbSet instead of Products
            return await _context.Suppliers.AnyAsync(predicate);
        }
    }
}
