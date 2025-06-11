using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.IRepositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<bool> ExistsAsync(Expression<Func<Transaction, bool>> value);

    }
}
