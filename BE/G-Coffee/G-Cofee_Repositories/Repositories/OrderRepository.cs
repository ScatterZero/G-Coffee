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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly GcoffeeDbContext _context;

        public OrderRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<Order> GetByOrderIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.ComboPackage)
                .FirstOrDefaultAsync(o => o.Id == (Guid)id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllOrderAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.ComboPackage)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> FindOrderAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.ComboPackage)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public Task<bool> ExistsAsync(Expression<Func<Order, bool>> value)
        {
            return _context.Orders.AnyAsync(value);
        }
    }
}
 

