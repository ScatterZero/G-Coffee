using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public string? ShortName { get; set; }
        public string UnitOfMeasureId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? SupplierId { get; set; } // Thêm lại SupplierId

    }

public class ReponseUpdateProductDto
    {
        public string ProductID { get; set; }

        public string ProductName { get; set; } = null!;

        public string? ShortName { get; set; }

        public string UnitOfMeasureId { get; set; } = null!;

        public decimal? UnitPrice { get; set; }

        public string? SupplierId { get; set; }

        public bool? IsDisabled { get; set; } = false;
    }
}
