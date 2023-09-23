using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ResponsibilityList:BaseEntity
    {

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        [ForeignKey("Manager")]
        public Guid ManagerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Manager Manager { get; set; }
    }
}
