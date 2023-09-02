using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WarehouseDetail:BaseEntity
    {
        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; }
        
        public double Price { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public UnitType UnitType { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }
}
