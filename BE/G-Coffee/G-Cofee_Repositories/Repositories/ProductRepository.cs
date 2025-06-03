using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

// Interface cho ProductRepository, kế thừa IGenericRepository


// Repository cho Products
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly GcoffeeDbContext _context;

    public ProductRepository(GcoffeeDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Expression<Func<Product, bool>> predicate)
    {
        return await _context.Products.AnyAsync(predicate);
    }

    public async Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(string supplierId)
    {
        return await FindAsync(p => p.SupplierId == supplierId);
    }


}