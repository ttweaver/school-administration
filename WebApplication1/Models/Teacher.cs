using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models

{
    public class Teacher
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]   
        public required string FirstName { get; set; } = string.Empty;
        [Display(Name = "Last Name")]
        public required string LastName { get; set; } = string.Empty;
        [Display(Name = "Contact Number")]
        public required string Contact { get; set; }
        public required string Qualifications { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }

        [Display(Name = "Courses Teaching")]
        public string? ClassesTaught { get; set; }
        

    }
}


