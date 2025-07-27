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
    public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
    {
        private readonly GcoffeeDbContext _context;
        public WarehouseRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(Expression<Func<Warehouse, bool>> value)
        {
            return await _context.Warehouses.AnyAsync(value);
        }
        public async Task<IEnumerable<Warehouse>> GetAllWarehouseAsync(string id)
        {
            return await _context.Warehouses.Where(t => t.TenantID == id)
                .Include(p => p.Tenant)
                .ToListAsync();
        }
    }
}
