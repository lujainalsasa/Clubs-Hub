using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalPro.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Display(Name = "First Name")]
        [Column(TypeName = "nvarchar(20)")]
        public String FirstName { get; set; } = String.Empty;

        [MinLength(3)]
        [MaxLength(30)]
        [Display(Name = "Last Name")]
        public String LastName { get; set; } = String.Empty;

        [EmailAddress]
        [Display(Prompt = "example@gmail.com")]
        public String Email { get; set; }= String.Empty;

        [Phone]
        public String Phone { get; set; }=String.Empty;
        [Display(Name = "Education Level")]
        public eEducationLevel EducationLevel { get; set; } = eEducationLevel.Junior;

        //Navigation
        public IEnumerable<Contest>? Contests { get; set; }
    }
}

