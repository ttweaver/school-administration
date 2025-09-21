using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public CreateModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["TeacherId"] = new SelectList(_context.Teacher, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        public List<string> TypeOptions { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Classroom.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public CreateModel()
        {
            TypeOptions = new List<string> { "Exam", "Quiz", "Assignment", "Project" };
        }
    }
}
