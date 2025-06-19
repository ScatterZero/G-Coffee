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
    }
}
