using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;

namespace G_Coffee_Services.IServices
{
    public interface IAccountService
    {
        Task<LoginResponseDTO?> LoginAsync(UserLoginDTO loginDto);
        Task RegisterAsync(UserRegisterDTO registerDto);
        Task<User> GetAccountByIdAsync(string id);
        Task<IEnumerable<User>> GetAllAccountsAsync();
        Task UpdateAccountAsync(string id, UserUpdateDTO dt);
        Task DeleteAccountAsync(string id);

    }
}
