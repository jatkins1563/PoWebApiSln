using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoWebApi.Models
{
    public class PoLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; } = 1;

        //foreign keys
        public int PurchaseOrderId { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        //end fk
    }
}
