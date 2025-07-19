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
}
