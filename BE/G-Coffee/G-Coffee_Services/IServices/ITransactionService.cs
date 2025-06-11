using G_Cofee_Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface ITransactionService 
    {
        Task ImportReceipt(TransactionDTO entity);
        Task<TransactionDTO> ExportReceipt(TransactionDTO entity);
    }
}
