using G_Cofee_Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface ITransactionDetailService
    {
        Task<TransactionDetailDTO> CreateTransactionDetailAsync(TransactionDetailDTO TransactionDetail);
        Task<TransactionDetailDTO> GetTransactionDetailByIdAsync(string id);
        Task<IEnumerable<TransactionDetailDTO>> GetAllTransactionDetailsAsync();
        Task UpdateTransactionDetailAsync(TransactionDetailDTO productDto);
        Task DeleteTransactionDetailAsync(string id);
    }
}
