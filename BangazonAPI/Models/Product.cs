using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(280, MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

    }
}
