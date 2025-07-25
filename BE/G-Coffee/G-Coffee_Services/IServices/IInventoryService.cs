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
        Task<Inventory> CreateInventoryAsync(InventoryDTO inventory);
        Task<Inventory> GetInventoryByIdAsync(string id);
        Task<IEnumerable<Inventory>> GetAllInventorysAsync();
        Task<InventoryUpdateDTO> UpdateInventoryAsync(InventoryUpdateDTO inventory);
        Task DeleteInventoryAsync(string id);
    }
}
