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
    public class TransactionDetailRepository : GenericRepository<TransactionDetail>, ITransactionDetailRepository
    {
        private readonly GcoffeeDbContext _context;
        public TransactionDetailRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> AnyAsync(Func<object, bool> value)
        {
            // This method seems to be incorrectly implemented. It should use a predicate instead of Func<object, bool>.
            // Assuming you want to check if any TransactionDetail exists based on a predicate.
            throw new NotImplementedException("Use ExistsAsync with an Expression<Func<TransactionDetail, bool>> instead.");
        }

        public async Task<bool> ExistsAsync(Expression<Func<TransactionDetail, bool>> predicate)
        {
            // Corrected to use the TransactionDetails DbSet instead of Suppliers
            return await _context.TransactionDetails.AnyAsync(predicate);
        }
    }
}
