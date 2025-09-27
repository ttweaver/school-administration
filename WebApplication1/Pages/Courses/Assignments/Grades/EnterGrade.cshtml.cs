using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Courses.Assignments.Grades
{
    public class EnterGradeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EnterGradeModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public AssignmentScore AssignmentScore { get; set; } = default!;

        public Student? Student { get; set; }
        public Assignment? Assignment { get; set; }
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGetAsync(int courseId, int assignmentId, int studentId)
        {
            CourseId = courseId;
            AssignmentId = assignmentId;
            StudentId = studentId;

            Assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            Student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            AssignmentScore = await _context.AssignmentScores
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId)
                ?? new AssignmentScore { AssignmentId = assignmentId, StudentId = studentId };

            if (Assignment == null || Student == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Retrieve the assignment for validation
            Assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == AssignmentScore.AssignmentId);

            if (Assignment == null)
            {
                ModelState.AddModelError(string.Empty, "Assignment not found.");
                return Page();
            }

            // Validate that PointsEarned does not exceed PointsPossible
            if (AssignmentScore.PointsEarned.HasValue && AssignmentScore.PointsEarned.Value > Assignment.PointsPossible)
            {
                ModelState.AddModelError(nameof(AssignmentScore.PointsEarned), $"Points earned cannot exceed points possible ({Assignment.PointsPossible}).");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            var existingScore = await _context.AssignmentScores
                .FirstOrDefaultAsync(s => s.AssignmentId == AssignmentScore.AssignmentId && s.StudentId == AssignmentScore.StudentId);

            if (existingScore == null)
            {
                _context.AssignmentScores.Add(AssignmentScore);
            }
            else
            {
                existingScore.PointsEarned = AssignmentScore.PointsEarned;
                existingScore.Comments = AssignmentScore.Comments;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { courseId = CourseId, assignmentId = AssignmentId });
        }
    }
}