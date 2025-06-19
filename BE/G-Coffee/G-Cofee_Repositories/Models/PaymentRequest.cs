using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Models
{
    public class PaymentRequest
    {
        public long OrderCode { get; set; } // Mã đơn hàng duy nhất (dùng timestamp)
        public int Amount { get; set; } // Số tiền (VND)
        public required string Description { get; set; } // Mô tả đơn hàng
        public required string CancelUrl { get; set; } // URL khi hủy
        public required string ReturnUrl { get; set; } // URL khi thành công
    }
}
