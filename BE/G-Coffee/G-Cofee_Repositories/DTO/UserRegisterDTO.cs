using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static G_Cofee_Repositories.Models.User;

namespace G_Cofee_Repositories.DTO
{
    public class UserRegisterDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        [EnumDataType(typeof(RoleEnum), ErrorMessage = "Role phải là 'User', 'Manager', hoặc 'Admin'.")]
        public string Role { get; set; } = null!;

    }
    public class UserUpdateDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        [EnumDataType(typeof(RoleEnum), ErrorMessage = "Role phải là 'User', 'Manager', hoặc 'Admin'.")]
        public string Role { get; set; } = null!;
        public bool? IsDisabled { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;


    }

}
