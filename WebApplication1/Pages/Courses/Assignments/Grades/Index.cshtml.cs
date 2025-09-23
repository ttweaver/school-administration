using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages.Courses.Assignments.Grades
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public List<AssignmentScore>? AssignmentGrades { get; set; }
		public List<Student> Students { get; set; } = new();

        public void OnGet(int courseId, int assignmentId)
        {
            CourseId = courseId;
            AssignmentId = assignmentId;
            Assignment = _context.Assignments.FirstOrDefault(a => a.Id == assignmentId && a.CourseId == courseId);
            AssignmentGrades = _context.AssignmentScores.Where(ag => ag.AssignmentId == assignmentId).ToList();

			// Get students enrolled in the course
			Students = _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student)
                .ToList();
        }
    }
}   