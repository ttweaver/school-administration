﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Classrooms
{
    public class DetailsModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public DetailsModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            else
            {
                Classroom = classroom;
            }
            return Page();
        }
    }
}
