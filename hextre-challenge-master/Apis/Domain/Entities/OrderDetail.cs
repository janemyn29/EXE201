using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetail: BaseEntity
    {
        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public bool ContactInDay { get; set; }
        public int TotalCall { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Guid? CancelReasonId { get; set; }
        public Guid WarehouseDetailId { get; set;}
        public double WarehousePrice { get; set; }
        public double ServicePrice { get; set; }
        public double TotalPrice { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public UnitType UnitType { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
