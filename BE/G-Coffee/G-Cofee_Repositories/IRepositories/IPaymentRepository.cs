using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.IRepositories
{
    public interface IPaymentRepository : IGenericRepository<Payment> 
    {
    }
}
