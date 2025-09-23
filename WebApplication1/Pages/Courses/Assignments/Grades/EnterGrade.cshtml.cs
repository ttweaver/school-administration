using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Courses.Assignments.Grades
{
    public class EnterGradeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EnterGradeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AssignmentScore AssignmentScore { get; set; } = default!;

        public Student? Student { get; set; }
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }

        public async Task<IActionResult> OnGetAsync(int courseId, int assignmentId, int studentId)
        {
            CourseId = courseId;
            AssignmentId = assignmentId;
            Student = await _context.Students.FindAsync(studentId);

			AssignmentScore = await _context.AssignmentScores
                .FirstOrDefaultAsync(ag => ag.AssignmentId == assignmentId && ag.StudentId == studentId)
                ?? new AssignmentScore { AssignmentId = assignmentId, StudentId = studentId };

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			
			AssignmentId = AssignmentScore.AssignmentId;
			CourseId = (await _context.Assignments.FindAsync(AssignmentScore.AssignmentId))?.CourseId ?? 0;
			if (!ModelState.IsValid)
            {
				Student = await _context.Students.FindAsync(AssignmentScore.StudentId);
				return Page();
            }

            var existing = await _context.AssignmentScores
                .FirstOrDefaultAsync(ag => ag.AssignmentId == AssignmentScore.AssignmentId && ag.StudentId == AssignmentScore.StudentId);

            if (existing != null)
            {
                existing.PointsEarned = AssignmentScore.PointsEarned;
                existing.Comments = AssignmentScore.Comments;
            }
            else
            {
                _context.AssignmentScores.Add(AssignmentScore);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { courseId = CourseId, assignmentId = AssignmentId });
        }
    }
}