using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Models
{
    public class WebhookData
    {
        public long OrderCode { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }



}
