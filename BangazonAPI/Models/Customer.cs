using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        public DateTime AcctEstablished { get; }

        
        public DateTime LastActiveDate { get; set; }

        [Required]
        public bool IsActive { get; set; }


        public List<Product> ProductsToSell { get; set; } = new List<Product>();

        public List<PaymentType> PaymentTypes { get; set; } = new List<PaymentType>();

        public List<Order> Orders { get; set; } = new List<Order>();


    }
}