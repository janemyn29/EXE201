using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServicePayment:BaseEntity
    {
        [ForeignKey("Contract")]
        public Guid ContractId { get; set; }


        public int MonthPayment { get; set; }
        public int YearPayment { get; set; }
        public double Price { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime ExtensionPeriod { get; set; }

        public PaymentType PaymentType { get; set; }

        public bool? ContactedInDay { get; set; }
        public bool IsPaid { get; set; }

        public virtual Contract Contract { get; set; }
        public IList<Transaction> Transactions { get; set; }
    }
}
