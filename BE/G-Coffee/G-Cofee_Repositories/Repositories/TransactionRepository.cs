﻿using G_Cofee_Repositories.IRepositories;
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
    public class TransactionRepository : GenericRepository<Models.Transaction>, ITransactionRepository
    {
        private readonly GcoffeeDbContext _context;
        public TransactionRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Transaction, bool>> value)
        {
            return await _context.Transactions.AnyAsync(value);
        }
    }
}
