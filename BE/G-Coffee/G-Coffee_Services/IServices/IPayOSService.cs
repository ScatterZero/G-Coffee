using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface IPayOSService
    {
        Task<PaymentResponse> CreatePaymentLink(PaymentRequest request);
        Task<PaymentDTO> CreatePaymentAsync(PaymentDTO Payment);
        Task<PaymentDTO> GetPaymentByIdAsync(string id);
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync();
        Task UpdatePaymentAsync(PaymentDTO Payment);
        Task DeletePaymentAsync(string id);  
    }
}
