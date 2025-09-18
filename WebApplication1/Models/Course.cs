using System;
using System.Collections.Generic;
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
        public string? Description { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        // Foreign key for Teacher
        [Required]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }
        // Navigation property for Teacher
        public required Teacher Teacher { get; set; }
        
        
    }
}