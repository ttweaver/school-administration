using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Courses.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public CreateModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

		public List<string> TypeOptions { get; set; } = new List<string> { "Assignment", "Exam", "Project", "Quiz" };
        public int CourseId { get; set; }

		public IActionResult OnGet(int courseId)
        {
            this.CourseId = courseId;
			ViewData["StudentId"] = new SelectList(_context.Students, "Id", "LastName");
            return Page();
        }

        [BindProperty]
        public Assignment Assignment { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int courseId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			Assignment.CourseId = courseId;
			_context.Assignments.Add(Assignment);
            await _context.SaveChangesAsync();

            return RedirectToPage($"/Courses/Assignments/Index", new { courseId });
        }
    }
}
