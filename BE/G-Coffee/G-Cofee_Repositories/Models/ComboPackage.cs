using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Models
{
    public class ComboPackage
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public required string Name { get; set; } // Tên gói (Basic, Pro, Premium)
        public int Price { get; set; } // Giá gói (VND)
        public required string Description { get; set; } // Mô tả gói
        //public required List<Order> Orders { get; set; } // Quan hệ với Order
    }
}
