using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Scores
{
    public class CreateModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public CreateModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<string> TitleOptions { get; set; } = new() { "Quiz", "Homework", "Exam", "Project" };
        public List<string> TypeOptions { get; set; } = new List<string> { "Exam", "Quiz", "Assignment", "Project" };

        public IActionResult OnGet()
        {
            ViewData["CourseId"] = new SelectList(_context.Classroom, "Id", "Name");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Score Score { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Score.Add(Score);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
