﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DepositPayment:BaseEntity
    {
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public virtual ApplicationUser Customer { get; set; }

    }
}
