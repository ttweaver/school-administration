using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [StringLength(100)]
        public required string Name { get; set; }
        [Display(Name = "Description")]
        [StringLength(500)]
        public required string? Description { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public required DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public required DateTime? EndDate { get; set; }
        [DisplayName("Year")]
        public int? Year { get { return this.StartDate?.Year; } }
        [DisplayName("Quarter")]
        public Quarter? Quarter
        {
            get
            {
                if (StartDate == null) return null;
                int month = StartDate.Value.Month;
                return QuarterHelper.GetQuarterFromMonth(month);
            }
        }

        // Foreign key for Teacher
        [Required]
        [Display(Name = "Teacher")]
        public required int TeacherId { get; set; }

        // Navigation property for Teacher
        public Teacher? Teacher { get; set; }

        // Navigation property for assignments (scores)
        public List<Assignment> Assignments { get; set; } = new();

        public List<Enrollment> Enrollments { get; set; } = new();
	}
}