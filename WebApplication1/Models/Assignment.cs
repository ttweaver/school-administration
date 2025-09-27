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
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [NotMapped]
        [Display(Name = "Points Possible")]
        public int PointsPossible
        {
            get
            {
                return Type switch
                {
                    AssignmentType.Assignment => 50,
                    AssignmentType.Quiz => 100,
                    AssignmentType.Project => 250,
                    AssignmentType.Exam => 500,
                    _ => 0
                };
            }
        }

        [StringLength(400)]
        public string? Comments { get; set; }

        // Navigation properties
        public Course? Course { get; set; }

        // Navigation property for assignment scores
        public List<AssignmentScore> AssignmentScores { get; set; } = new();
    }
}