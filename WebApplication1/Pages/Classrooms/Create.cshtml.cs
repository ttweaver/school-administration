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
    public class CreateModel(ApplicationDbContext context) : PageModel
    {
        private readonly ApplicationDbContext _context = context;

        [BindProperty]
        public Classroom Classroom { get; set; } = default!;
        public List<SelectListItem>? TeacherList { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TeacherList = await _context.Teacher
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.LastName
                })
                .ToListAsync();
            TeacherList.Insert(0, new SelectListItem { Value = "", Text = "(teacher)", Selected = true });
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TeacherList = await _context.Teacher
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.LastName
                    })
                    .ToListAsync();
                TeacherList.Insert(0, new SelectListItem { Value = "", Text = "(No teacher)" });
                return Page();
            }

            _context.Classroom.Add(Classroom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}