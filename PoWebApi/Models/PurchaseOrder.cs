using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PoWebApi.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        [Required, StringLength(80)]
        public string Description { get; set; }
        [Required, StringLength(20)]
        public string Status { get; set; } = "NEW"; //new->edit->review->approved/rejected
        [Column(TypeName = "decimal(9, 2)")]
        public decimal Total { get; set; } = 0;
        public bool Active { get; set; } = true;

        //foreign key
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        //end fk

    }
}
