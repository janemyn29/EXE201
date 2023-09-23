using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer:BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public IList<Contract> Contracts { get; set; }
        public IList<ResponsibilityList> ResponsibilityLists { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public IList<Feedback> Feedbacks { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
        public IList<Request> Requests { get; set; }
    }
}
