using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
   
        public class OrderDTO
        {
            public Guid Id { get; set; } = Guid.NewGuid(); // Tự động tạo ID mới nếu không có
            public long OrderCode { get; set; }
            public string UserId { get; set; } = string.Empty; // Tránh null, sẽ được gán từ UserId của người dùng
              public Guid ComboPackageId { get; set; }
            public int Amount { get; set; }
            public string Status { get; set; } = string.Empty; // Tránh null
            public string CheckoutUrl { get; set; } = string.Empty; // Tránh null
        }
    
}
