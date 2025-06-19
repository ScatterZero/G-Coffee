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
    public class ComboPackageRepository : GenericRepository<ComboPackage>, IComboPackageRepository
    {
        private readonly GcoffeeDbContext _context;
        public ComboPackageRepository(GcoffeeDbContext context) : base(context)
        {
        }
        public async Task<bool> ExistsAsync(Expression<Func<Models.ComboPackage, bool>> value)
        {
            return await _context.ComboPackages.AnyAsync(value);
        }
    }
}
