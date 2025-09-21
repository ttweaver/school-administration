using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.UI;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Classrooms
{
    public class EditModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public EditModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Classroom Classroom { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classroom.FirstOrDefaultAsync(m => m.Id == id);
            if (classroom == null)
            {
                return NotFound();
            }
            Classroom = classroom;
            var teachers = _context.Teacher
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.LastName  .ToString()
                })
                .ToList();
            teachers.Insert(0, new SelectListItem { Value = "", Text = "(No teacher)" });
            ViewData["TeacherId"] = new SelectList(teachers, "Value", "Text", null);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrEmpty(Request.Form["TeacherId"]))
            {
                Classroom.TeacherId = null;
            }

            _context.Attach(Classroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(Classroom.Id))
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

        private bool ClassroomExists(int id)
        {
            return _context.Classroom.Any(e => e.Id == id);
        }
    }
}

