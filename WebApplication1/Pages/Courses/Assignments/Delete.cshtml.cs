using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Courses.Assignments
{
    public class DeleteModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public DeleteModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Assignment Score { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Assignments.Include(c => c.Course).FirstOrDefaultAsync(m => m.Id == id);

            if (score == null)
            {
                return NotFound();
            }
            else
            {
                Score = score;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Assignments.FindAsync(id);
            if (score != null)
            {
                Score = score;
                _context.Assignments.Remove(Score);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
