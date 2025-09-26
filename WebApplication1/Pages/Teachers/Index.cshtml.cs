using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public IList<Teacher> Teacher { get; set; } = new List<Teacher>();
        public string? Search { get; set; }
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? SortOrder { get; set; }

        public async Task OnGetAsync(string? search, int? pageNumber, int? pageSize, string? sortOrder)
        {
            Search = search;
            PageSize = pageSize ?? 10;
            CurrentPage = pageNumber ?? 1;
            SortOrder = sortOrder;

            var teachers = _context.Teachers.AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                teachers = teachers.Where(t =>
                    t.FirstName.Contains(Search) ||
                    t.LastName.Contains(Search) ||
                    t.Email.Contains(Search) ||
                    t.Qualifications.Contains(Search));
            }

            teachers = SortOrder switch
            {
                "FirstName_Desc" => teachers.OrderByDescending(t => t.FirstName),
                "LastName" => teachers.OrderBy(t => t.LastName),
                "LastName_Desc" => teachers.OrderByDescending(t => t.LastName),
                "Email" => teachers.OrderBy(t => t.Email),
                "Email_Desc" => teachers.OrderByDescending(t => t.Email),
                "Qualifications" => teachers.OrderBy(t => t.Qualifications),
                "Qualifications_Desc" => teachers.OrderByDescending(t => t.Qualifications),
                _ => teachers.OrderBy(t => t.FirstName)
            };

            var count = await teachers.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            Teacher = await teachers
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
