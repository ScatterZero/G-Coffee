using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Models
{
    public class Order
    {
        public Guid Id { get; set; } = new Guid(); // ID tự tăng 
        public long OrderCode { get; set; }
        [ForeignKey("ComboPackage")]

        public Guid ComboPackageId { get; set; } // Liên kết với gói combo
        public int Amount { get; set; } // Giá gói

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public required string Status { get; set; } 
        public  string? CheckoutUrl { get; set; }  // URL thanh toán
        public DateTime CreatedAt { get; set; } // Thời gian tạo
        public virtual ComboPackage? ComboPackage { get; set; } // Quan hệ với gói combo

        public virtual User? User { get; set; } // Quan hệ với người dùng
    }
}
