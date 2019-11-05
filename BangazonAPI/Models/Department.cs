﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Department
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string DeptName { get; set;}

        [Required]
        public double Budget { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();


    }
}
