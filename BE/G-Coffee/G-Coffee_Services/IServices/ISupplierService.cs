using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface ISupplierService
    {      
        Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplier);
        Task<SupplierDTO> GetSupplierByIdAsync(string id);
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync();
        Task UpdateSupplierAsync(SupplierDTO productDto);
        Task DeleteSupplierAsync(string id);
    }
}
