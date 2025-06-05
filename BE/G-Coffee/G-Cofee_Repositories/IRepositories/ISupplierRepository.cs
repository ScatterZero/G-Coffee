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
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        //Task AddAsync(SupplierDTO entity);
        //Task DeleteAsync(Supplier supplier);
        Task<bool> ExistsAsync(Expression<Func<Supplier, bool>> value);
    }
}
