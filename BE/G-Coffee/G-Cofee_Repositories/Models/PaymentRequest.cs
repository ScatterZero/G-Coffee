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
        public  string? Description { get; set; } // Mô tả đơn hàng
        public  string? CancelUrl { get; set; } = "https://my.payos.vn/e880257ecd8e11ef943f0242ac110002/create-payment-link"; // URL khi hủy
        public  string? ReturnUrl { get; set; } = "https://my.payos.vn/e880257ecd8e11ef943f0242ac110002/create-payment-link";// URL khi thành công
    }

}
