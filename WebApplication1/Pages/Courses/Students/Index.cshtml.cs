using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Courses.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public List<Student> Students { get; set; } = new();
        public Dictionary<int, (double Earned, double Possible)> Grades { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            CourseId = courseId;
            var course = await _context.Courses
                .Include(c => c.Assignments)
                .ThenInclude(a => a.AssignmentScores)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound();

            CourseName = course.Name;

            // Get enrolled students
            Students = await _context.Enrollments
                .Where(e => e.Course.Id == courseId)
                .Select(e => e.Student)
                .Distinct()
                .ToListAsync();

            // Use GradeCalculator for each student
            foreach (var student in Students)
            {
                var (earned, possible) = GradeCalculator.CalculateStudentGrade(student.Id, course.Assignments);
                Grades[student.Id] = (earned, possible);
            }

            return Page();
        }
    }
}