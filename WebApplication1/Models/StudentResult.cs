using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
   public abstract class StudentResult
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Date Awarded")]
        public DateTime DateAwarded { get; set; } = DateTime.Now;

        [StringLength(400)]
        public string? Comments { get; set; }
    }
}
