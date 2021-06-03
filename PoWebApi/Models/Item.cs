using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PoWebApi.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required, StringLength(30)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price { get; set; } = 0;
        public bool Active { get; set; } = true;
    }
}
