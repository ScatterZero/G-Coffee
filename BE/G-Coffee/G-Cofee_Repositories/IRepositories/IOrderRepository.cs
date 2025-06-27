using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<bool> ExistsAsync(Expression<Func<Order, bool>> value);

    }


}
