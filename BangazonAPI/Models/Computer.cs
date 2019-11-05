using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Computer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Make { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Manufacturer { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
        public DateTime DecommissionDate { get; set; }
    }
}
