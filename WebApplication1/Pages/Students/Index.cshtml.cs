using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public IList<Student> Student { get; set; } = new List<Student>();
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

            var students = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                students = students.Where(s =>
                    s.FirstName.Contains(Search) ||
                    s.LastName.Contains(Search) ||
                    s.Email.Contains(Search));
            }

            // Sorting
            students = SortOrder switch
            {
                "FirstName_Desc" => students.OrderByDescending(s => s.FirstName),
                "LastName" => students.OrderBy(s => s.LastName),
                "LastName_Desc" => students.OrderByDescending(s => s.LastName),
                "DateOfBirth" => students.OrderBy(s => s.DateOfBirth),
                "DateOfBirth_Desc" => students.OrderByDescending(s => s.DateOfBirth),
                "Email" => students.OrderBy(s => s.Email),
                "Email_Desc" => students.OrderByDescending(s => s.Email),
                _ => students.OrderBy(s => s.FirstName)
            };

            var count = await students.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            Student = await students
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
