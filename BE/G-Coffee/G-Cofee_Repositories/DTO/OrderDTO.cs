using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
   
        public class OrderDTO
        {
            public string UserId { get; set; } = string.Empty; // Tránh null, sẽ được gán từ UserId của người dùng
            public Guid ComboPackageId { get; set; }
        }
    
}
