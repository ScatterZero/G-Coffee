using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;

namespace G_Cofee_Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByNameAsync(string username);
        Task AddUserAsync(User user);
    }
}
