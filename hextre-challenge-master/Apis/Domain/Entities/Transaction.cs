﻿using Domain.Enums;
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
        public Guid PaymentId { get; set; }
        public double Amount { get; set; }
        public string IpnURL { get; set; }
        public string Info { get; set; }
        public string PartnerCode { get; set; }
        public string RedirectUrl { get; set; }
        public string RequestId { get; set; }
        public string RequestType { get; set; }
        public DepositStatus Status { get; set; }
        public string PaymentMethod { get; set; }

        public string orderIdFormMomo { get; set; }
        public string orderType { get; set; }
        public long transId { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; }
        public string? payType { get; set; }
        public long responseTime { get; set; }
        public string extraData { get; set; }
        public string signature { get; set; }

        public virtual ServicePayment ServicePayment { get; set; }
    }
}
