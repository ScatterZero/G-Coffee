using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using G_Cofee_Repositories.Models;
namespace G_Coffee_Services.IServices
{
    public interface ITransactionService 
    {
        Task<G_Cofee_Repositories.Models.Transaction> ImportReceipt(TransactionDTO entity);
        Task<G_Cofee_Repositories.Models.Transaction> ExportReceipt(TransactionDTO entity);
        Task<G_Cofee_Repositories.Models.Transaction> GetTransactionByIdAsync(string transactionId);
        Task<IEnumerable<G_Cofee_Repositories.Models.Transaction>> GetAllTransactionsAsync();
    }
}
