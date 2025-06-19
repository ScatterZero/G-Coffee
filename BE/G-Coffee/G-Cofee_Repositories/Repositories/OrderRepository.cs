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
    public class OrderRepository : GenericRepository<Order>, IOrderRerpository
    {
        private readonly GcoffeeDbContext _context;
        public OrderRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(Expression<Func<Order, bool>> predicate)
        {
            // Corrected to use the Suppliers DbSet instead of Products
            return await _context.Orders.AnyAsync(predicate);
        }
    }
 
}
