using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Contract:BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public string CustomerId { get; set; }
        [ForeignKey("OrderDetail")]
        public string OrderDetailId { get; set; }
        public string StaffId { get; set; }
        public DateTime  StartTime{ get; set; }
        public DateTime  EndTime{ get; set; }
        public string  File{ get; set; }
        public string  Description{ get; set; }
        public double ServicePrice { get; set; }
        public double WarehousePrice { get; set; }
        public double TotalPrice { get; set; }
        public double Deposit { get; set; }
        public ContractStatus ContractStatus { get; set; }



        public IList<ServicePayment> ServicePayments { get; set; }


        public virtual ApplicationUser Customer { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
    }
}
