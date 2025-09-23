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

		public async Task<IActionResult> OnGetAsync(int courseId)
        {
            CourseId = courseId;
			Assignments = await _context.Assignments
                .Include(s => s.Course)
                .Where(s => s.CourseId == courseId)
                .ToListAsync();

            return Page();
        }
    }
}
