using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface IComboPackageService 
    {
        Task<ComboPackage> CreateComboPackageAsync(ComboPackage comboPackage);
        Task<ComboPackage> GetComboPackageByIdAsync(string id);
        Task<IEnumerable<ComboPackage>> GetAllComboPackagesAsync();
        Task UpdateComboPackageAsync(ComboPackage comboPackage);
        Task DeleteComboPackageAsync(string id);
    }
}
