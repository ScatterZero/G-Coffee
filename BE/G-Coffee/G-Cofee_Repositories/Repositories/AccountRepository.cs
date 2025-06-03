using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace G_Cofee_Repositories.Repositories
{
    public class AccountRepository :GenericRepository<User>, IAccountRepository
    {
        private readonly GcoffeeDbContext _dbContext;

        public AccountRepository(GcoffeeDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); // cho phép null để có thể thực hiện một số tính năng
        }

        public async Task AddUserAsync(User user)
        {
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByNameAsync(string username)
        {
            //if (_dbContext?.Users == null) 
            //    return null;
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
