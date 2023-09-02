using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order:BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public string CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public bool ContactInDay { get; set; }
        public int TotalCall { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Guid? CancelReasonId { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public IList<OrderDetail> OrderDetails { get;}
    }
}
