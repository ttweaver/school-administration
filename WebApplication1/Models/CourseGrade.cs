using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CourseGrade : StudentResult
    {
        [Required]
        public int CourseId { get; set; }

        public Course? Course { get; set; }

		[Required]
		[Display(Name = "Letter Grade")]
		public LetterGrade Letter { get; set; }
	}
}
