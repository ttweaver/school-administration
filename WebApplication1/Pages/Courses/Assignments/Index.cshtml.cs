using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Pages.Courses.Assignments
{
    public class IndexModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public IndexModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Assignment> Assignments { get; set; } = default!;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;

		public async Task<IActionResult> OnGetAsync(int courseId)
        {
            CourseId = courseId;
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return NotFound();
            }

            CourseName = course.Name;

            Assignments = await _context.Assignments
                .Where(a => a.CourseId == courseId)
                .Include(a => a.Course)
                .ToListAsync();

            return Page();
        }
    }
}
