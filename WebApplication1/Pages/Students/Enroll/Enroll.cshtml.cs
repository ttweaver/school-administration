using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Enroll
{
    public class EnrollModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EnrollModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Enrollment Enrollment { get; set; } = new();

        public List<Course> Course { get; set; } = new();
        public HashSet<int> EnrolledCourseIds { get; set; } = new();

        public void OnGet(int id)
        {
			var today = DateTime.Today;
			var nextQuarter = QuarterHelper.GetNextQuarter(today);

			Enrollment.StudentId = id;
            var courses = _context.Courses
                .AsEnumerable() // Switch to LINQ-to-Objects
                .Where(e => QuarterHelper.GetQuarterFromDate(e.StartDate) == nextQuarter)
                .ToList();
            Course = courses;
            EnrolledCourseIds = _context.Enrollments
                .Where(e => e.StudentId == id)
                .Select(e => e.CourseId)
                .ToHashSet();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                Course = _context.Courses.ToList();
                EnrolledCourseIds = _context.Enrollments
                    .Where(e => e.StudentId == id)
                    .Select(e => e.CourseId)
                    .ToHashSet();
                return Page();
            }

            Enrollment.StudentId = id;
            _context.Enrollments.Add(Enrollment);
            await _context.SaveChangesAsync();
			return RedirectToPage(new { id });
		}

        public async Task<IActionResult> OnPostUnenrollAsync(int id)
        {
            // id is studentId from the route
            int courseId = Convert.ToInt32(Request.Form["Enrollment.CourseId"]);
            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.StudentId == id && e.CourseId == courseId);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            // Refresh the page for the same student
            return RedirectToPage(new { id });
        }
    }
}