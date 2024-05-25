using System.ComponentModel.DataAnnotations;

namespace FinalPro.Models
{
    public class Contest
    {

        public int ContestId { get; set; }

        public int ClubId { get; set; }
        public int StudentId { get; set; }

        [Display(Name = "Contest Name")]
        public String Name { get; set; }

        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        public eRole Role { get; set; }

        //Navigation
        public Club? Club { get; set; }
        public Student? Student { get; set; }
    }
}
