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
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        [ForeignKey("RentWarehouse")]
        public Guid RentWarehouseId { get; set; }
        [ForeignKey("Manager")]
        public Guid ManagerId { get; set; }
        public Guid OrderDetailId { get; set; }
        public Guid ProviderId { get; set; }
        public DateTime  StartTime{ get; set; }
        public DateTime  EndTime{ get; set; }
        public string  File{ get; set; }
        public string  Description{ get; set; }
        public double ServicePrice { get; set; }
        public double WarehousePrice { get; set; }
        public double TotalPrice { get; set; }
        public double DepositFee { get; set; }
        public ContractStatus ContractStatus { get; set; }
        

        public IList<ServicePayment> ServicePayments { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual RentWarehouse RentWarehouse { get; set; }
        public virtual Manager Manager { get; set; }
    }
}
