using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class TrainingProgram
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ProgramName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int MaxAttendees { get; set; }

        public List<Employee> CurrentAttendees { get; set; }

    }
}
