using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Repositories
{
    public class UnitOfMeasureRepository : GenericRepository<UnitsOfMeasure>, IUnitOfMeasureRepository
    {
        private readonly GcoffeeDbContext _context;
        public UnitOfMeasureRepository(GcoffeeDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
