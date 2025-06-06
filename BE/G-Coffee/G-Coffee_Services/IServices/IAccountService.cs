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
        Task<string?> LoginAsync(UserLoginDTO loginDto);
        Task RegisterAsync(UserRegisterDTO registerDto);
    }
}
