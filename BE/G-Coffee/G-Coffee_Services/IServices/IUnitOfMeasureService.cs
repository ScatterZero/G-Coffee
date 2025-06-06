using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface IUnitOfMeasureService 
    {
        Task<UnitOfMeasureDTO> CreateUnitOfMeasureAsync(UnitOfMeasureDTO UnitOfMeasure);
        Task<UnitOfMeasureDTO> GetUnitOfMeasureByIdAsync(string id);
        Task<IEnumerable<UnitOfMeasureDTO>> GetAllUnitOfMeasuresAsync();
        Task UpdateUnitOfMeasureAsync(UnitOfMeasureDTO unitOfMeasure);
        Task DeleteUnitOfMeasureAsync(string id);
    }
}
