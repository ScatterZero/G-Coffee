using G_Cofee_Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface IWarehouseService
    {
        Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO Warehouse);
        Task<WarehouseDTO> GetWarehouseByIdAsync(string id);
        Task<IEnumerable<WarehouseDTO>> GetAllWarehousesAsync();
        Task UpdateWarehouseAsync(WarehouseDTO Warehouse);
        Task DeleteWarehouseAsync(string id);
    }
}
