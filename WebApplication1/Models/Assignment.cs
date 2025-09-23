using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public enum AssignmentType
    {
        Assignment = 0,
        Quiz = 1,
        Exam = 2,
        Project = 3
    }

    public class Assignment
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Assignment Type")]
        public AssignmentType Type { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

		[Required]
		[StringLength(500)]
		public required string Description { get; set; }

		[Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Assigned")]
        public DateTime DateAssigned { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Points possible must be greater than 0.")]
        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Points Possible")]
        public decimal PointsPossible { get; set; }

        [StringLength(400)]
        public string? Comments { get; set; }

        // Navigation properties
        public Course? Course { get; set; }
    }
}