using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.IRepositories
{
    public interface ITransactionDetailRepository : IGenericRepository<TransactionDetail>
    {
        Task<bool> AnyAsync(Func<object, bool> value);
        Task<bool> ExistsAsync(Expression<Func<TransactionDetail, bool>> value);

    }
}
