using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AssignmentScore : StudentResult
    {
        [Required]
        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        [Range(0, 100, ErrorMessage = "Grade value must be between 0 and 100.")]
        [DisplayName("Points Earned")]
        public int? PointsEarned { get; set; }
        public bool IsGraded => PointsEarned != null;
    }
}
