﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Feedback:BaseEntity
    {
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; }

        public string Rating { get; set; }
        public string FeedbackText { get; set;}

        public virtual Customer Customer { get; set; }
        public virtual Warehouse Warehouse { get; set; }

    }
}
