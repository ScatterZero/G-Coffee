using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.IRepositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly GcoffeeDbContext _context;
        public PaymentRepository(GcoffeeDbContext context) : base(context)
        {
        }
        public async Task<bool> ExistsAsync(Expression<Func<Payment, bool>> value)
        {
            return await _context.Payments.AnyAsync(value);
        }
    }
    
    
}
