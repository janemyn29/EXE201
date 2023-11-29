using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderViewModels
{
    public class OrderStatic
    {
        public int totalOrder { get; set; }
        public int SuccessPaymentOrder { get; set; }
        public int FailPaymentOrder { get; set; }
        public int PendingOrder { get; set; }

        public double TotalRevenue { get; set; }
        public double ServiceRevenue { get; set; }
        public double WarehouseRevenue { get; set; }
    }
}
