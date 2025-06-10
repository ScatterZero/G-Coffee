using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class WarehouseDTO
    {
        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; } = null!;

        public string? Address { get; set; }

        public string? ManagerId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
