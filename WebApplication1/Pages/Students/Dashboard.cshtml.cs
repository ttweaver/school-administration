using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Security.Claims;

namespace WebApplication1.Pages.Students
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Course> EnrolledCourses { get; set; } = new();
        public List<Teacher> CurrentTeachers { get; set; } = new();
        public Dictionary<int, string> CourseGrades { get; set; } = new();
        public Dictionary<int, List<AssignmentInfo>> AssignmentsByCourse { get; set; } = new();
        public Dictionary<int, string> CourseNames { get; set; } = new();
        public string StudentFirstName { get; set; } = "";

        public class AssignmentInfo
        {
            public string Title { get; set; } = "";
            public DateTime? DueDate { get; set; }
            public decimal PointsPossible { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int studentId)
        {
            // Get the student and set the first name
            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            // Get the logged-in user's email
            var userEmail = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.Email)
                : null;

            // Only allow access if the logged-in user's email matches the student's email
            if (string.IsNullOrEmpty(userEmail) || !string.Equals(userEmail, student.Email, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            StudentFirstName = student.FirstName;

            // Get enrolled courses
            EnrolledCourses = await _context.Courses
                .Include(c => c.Teacher)
                .Where(c => c.Enrollments.Any(e => e.StudentId == studentId))
                .ToListAsync();

            // Get current teachers
            CurrentTeachers = EnrolledCourses
                .Select(c => c.Teacher)
                .Where(t => t != null)
                .Distinct()
                .ToList()!;

            // Get course grades using GradeCalculator
            foreach (var course in EnrolledCourses)
            {
                var assignments = await _context.Assignments
                    .Include(a => a.AssignmentScores)
                    .Where(a => a.CourseId == course.Id)
                    .ToListAsync();

                var (earned, possible) = GradeCalculator.CalculateStudentGrade(studentId, assignments);
                CourseGrades[course.Id] = GradeCalculator.GetLetterGrade(earned, possible);
                CourseNames[course.Id] = course.Name;
            }

            // Get assignments for each course
            foreach (var course in EnrolledCourses)
            {
                var assignments = await _context.Assignments
                    .Where(a => a.CourseId == course.Id)
                    .ToListAsync();

                var assignmentInfos = new List<AssignmentInfo>();
                foreach (var a in assignments)
                {
                    var ag = await _context.AssignmentScores
                        .FirstOrDefaultAsync(g => g.AssignmentId == a.Id && g.StudentId == studentId);
                    assignmentInfos.Add(new AssignmentInfo
                    {
                        Title = a.Title,
                        DueDate = a.DueDate,
                        PointsPossible = a.PointsPossible,
                    });
                }
                AssignmentsByCourse[course.Id] = assignmentInfos;
            }

            return Page();
        }
    }
}