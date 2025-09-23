using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Navigation properties
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}