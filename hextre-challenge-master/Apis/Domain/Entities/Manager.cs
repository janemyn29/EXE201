﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Manager:BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public IList<Contract> Contracts { get; set; }
        public IList<ResponsibilityList> ResponsibilityLists { get; set; }

    }
}
