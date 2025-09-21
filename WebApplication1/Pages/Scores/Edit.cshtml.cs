using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Pages.Scores
{
    public class EditModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public EditModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
            TitleOptions = new List<string>(); // Initialize to avoid CS8618
        }

        [BindProperty]
        public Score Score { get; set; } = default!;

        // Add this property for your dropdown options
        public SelectList TypeOptions { get; set; }
        public List<string> TitleOptions { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Score.FirstOrDefaultAsync(m => m.Id == id);
            if (score == null)
            {
                return NotFound();
            }
            Score = score;
            ViewData["CourseId"] = new SelectList(_context.Classroom, "Id", "Name");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id");

            TitleOptions = new List<string> { "Quiz", "Exam", "Assignment", "Project" };

            // Use enum values for TypeOptions
            TypeOptions = new SelectList(Enum.GetValues(typeof(ScoreType)));

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Score).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoreExists(Score.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ScoreExists(int id)
        {
            return _context.Score.Any(e => e.Id == id);
        }
    }
}
