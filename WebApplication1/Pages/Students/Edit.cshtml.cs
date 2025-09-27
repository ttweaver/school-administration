using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WebApplication1.Data;
using WebApplication1.Models;
using System.IO;

namespace WebApplication1.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public EditModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        [BindProperty]
        public IFormFile? ProfilePicture { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student =  await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            Student = student;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var studentToUpdate = await _context.Students.FindAsync(Student.Id);
            if (studentToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(studentToUpdate, "Student"))
            {
                // Handle profile picture upload to database
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await ProfilePicture.CopyToAsync(ms);
                        studentToUpdate.ProfilePicture = ms.ToArray();
                        studentToUpdate.ProfilePictureContentType = ProfilePicture.ContentType;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
