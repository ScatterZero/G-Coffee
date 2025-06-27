using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class SupplierDTO
    {
        public string SupplierId { get; set; } = null!;

        public string SupplierName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? ContactPerson { get; set; }

    }
}
