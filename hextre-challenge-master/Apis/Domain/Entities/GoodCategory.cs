﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GoodCategory:BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IList<Good> Goods { get; set; }
    }
}
