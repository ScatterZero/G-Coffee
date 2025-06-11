using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class InventoryDTO
    {
        public Guid InventoryId { get; set; }

        public string WarehouseId { get; set; } = null!;

        public string ProductID { get; set; } = null!;

        public decimal? Quantity { get; set; }
        public DateTime? LastUpdated { get; set; }

    }
}
