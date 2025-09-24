using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public IList<Student> Student { get; set; } = new List<Student>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync(int? pageNumber, int? pageSize, string? search)
        {
            PageSize = pageSize ?? PageSize;
            Search = search ?? Search;

            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(Search) ||
                    s.LastName.Contains(Search) ||
                    s.Email.Contains(Search));
            }

            var totalStudents = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalStudents / (double)PageSize);
            CurrentPage = pageNumber ?? 1;

            Student = await query
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
