using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaction:BaseEntity
    {
        [ForeignKey("ServicePayment")]
        public Guid ServicePaymentId { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public virtual ServicePayment ServicePayment { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
