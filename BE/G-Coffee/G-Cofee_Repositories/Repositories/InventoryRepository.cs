using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Repositories
{
    public class InventoryRepository : GenericRepository<Models.Inventory>, IInventoryRepository
    {
        private readonly GcoffeeDbContext _context;
        public InventoryRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(Expression<Func<Models.Inventory, bool>> value)
        {
            return await _context.Inventories.AnyAsync(value);
        }

        public async Task<Inventory> GetByProductAndWarehouseAsync(string productId, string warehouseId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(warehouseId))
                throw new ArgumentException("ProductId and WarehouseId cannot be null or empty.");

            return await GetByStringIdAsync(i => i.ProductID == productId && i.WarehouseId == warehouseId, cancellationToken);
        }
        public async Task<Inventory> GetByInventoryIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
         
            if (Id == Guid.Empty)
                throw new ArgumentException("Inventory ID cannot be empty.", nameof(Id));
            return await _context.Inventories
                .Include(i => i.Warehouse)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.InventoryId == Id, cancellationToken);
        }


        public async Task<IEnumerable<Inventory>> GetAllInventory(string tentID)
        {
            return await _context.Inventories.Where(t => t.TenantID == tentID)
                .Include(i => i.Warehouse)
                .Include(i => i.Product)
                .ToListAsync();
        }
    }

}

