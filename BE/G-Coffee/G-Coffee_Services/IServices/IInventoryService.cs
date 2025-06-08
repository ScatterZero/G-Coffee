using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface IInventoryService 
    {
        Task<InventoryDTO> CreateInventoryAsync(InventoryDTO inventory);
        Task<InventoryDTO> GetInventoryByIdAsync(string id);
        Task<IEnumerable<InventoryDTO>> GetAllInventorysAsync();
        Task UpdateInventoryAsync(InventoryDTO inventory);
        Task DeleteInventoryAsync(string id);
    }
}
