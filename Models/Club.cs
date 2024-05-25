using System.ComponentModel.DataAnnotations;
using System;

namespace FinalPro.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Please Enter Club Name!")]
        public String Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        //Navigation
        public IEnumerable<Contest>? Contests { get; set; }
    }
}
