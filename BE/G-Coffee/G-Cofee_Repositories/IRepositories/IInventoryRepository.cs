﻿using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.IRepositories
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        Task<bool> ExistsAsync(Expression<Func<Inventory, bool>> value);
        Task<Inventory> GetByProductAndWarehouseAsync(string productId, string warehouseId, CancellationToken cancellationToken = default);


    }
}
