using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public IndexModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }
        public int CurrentPage => PageNumber;

        public async Task OnGetAsync()
        {
            var query = _context.Courses.Include(c => c.Teacher).AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(Search))
            {
                query = query.Where(c =>
                    c.Name.Contains(Search) ||
                    c.Description.Contains(Search) ||
                    c.Teacher.LastName.Contains(Search));
            }

            // Sort
            query = SortOrder switch
            {
                "Name" => query.OrderBy(c => c.Name),
                "Name_Desc" => query.OrderByDescending(c => c.Name),
                "Description" => query.OrderBy(c => c.Description),
                "Description_Desc" => query.OrderByDescending(c => c.Description),
                "StartDate" => query.OrderBy(c => c.StartDate),
                "StartDate_Desc" => query.OrderByDescending(c => c.StartDate),
                "EndDate" => query.OrderBy(c => c.EndDate),
                "EndDate_Desc" => query.OrderByDescending(c => c.EndDate),
                "Quarter" => query.OrderBy(c => c.Quarter),
                "Quarter_Desc" => query.OrderByDescending(c => c.Quarter),
                "Year" => query.OrderBy(c => c.Year),
                "Year_Desc" => query.OrderByDescending(c => c.Year),
                "Teacher" => query.OrderBy(c => c.Teacher.LastName),
                "Teacher_Desc" => query.OrderByDescending(c => c.Teacher.LastName),
                _ => query.OrderBy(c => c.Name)
            };

            // Paging
            int totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            Course = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
