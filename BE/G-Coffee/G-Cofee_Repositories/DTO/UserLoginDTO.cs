using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static G_Cofee_Repositories.Models.User;

namespace G_Cofee_Repositories.DTO
{
    public class UserLoginDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class LoginResponseDTO
    {
        public string? Username { get; set; } = null!;
        public string? AccessToken { get; set; } = null!;
        public int ExpiresIn { get; set; }
        public string? Role { get; set; } 
    }
}
