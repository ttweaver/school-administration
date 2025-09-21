using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public enum ScoreType
    {
        Assignment = 0,
        Quiz = 1,
        Exam = 2,
        Project = 3
    }

    public class Score
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Type")]
        public ScoreType Type { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Assigned")]
        public DateTime DateAssigned { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Points possible must be greater than 0.")]
        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Points Possible")]
        public decimal PointsPossible { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Points earned cannot be negative.")]
        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Points Earned")]
        public decimal PointsEarned { get; set; }

        [StringLength(400)]
        public string? Comments { get; set; }

        // Navigation properties
        public required Student Student { get; set; }
        public required Course Course { get; set; }
    }
}