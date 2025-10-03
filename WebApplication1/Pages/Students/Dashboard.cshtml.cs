using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        public int StudentId { get; set; }
        public byte[]? StudentProfilePicture { get; set; }
        public string? StudentProfilePictureContentType { get; set; }
        public string CurrentQuarterName { get; set; } = "";
        public int CurrentQuarterYear { get; set; }

        public class AssignmentInfo
        {
            public string Title { get; set; } = "";
            public DateTime? DueDate { get; set; }
            public int PointsPossible { get; set; }
            public int PointsEarned { get; set; }
            public string Grade { get; set; } = "-";
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

            StudentId = studentId;
            StudentFirstName = student.FirstName;
            StudentProfilePicture = student.ProfilePicture;
            StudentProfilePictureContentType = student.ProfilePictureContentType;

            // Get enrolled courses
            var currentQuarterDate = QuarterHelper.QuarterDates()
                .FirstOrDefault(qd => QuarterHelper.GetQuarterFromDate(DateTime.Today) == Enum.Parse<Quarter>(qd.Name));
            if (currentQuarterDate != null)
            {
                CurrentQuarterName = currentQuarterDate.Name;
                CurrentQuarterYear = currentQuarterDate.StartDate.Year;
            }

            var allEnrolledCourses = await _context.Courses
                .Include(c => c.Teacher)
                .Where(c => c.Enrollments.Any(e => e.StudentId == studentId))
                .ToListAsync();

            EnrolledCourses = allEnrolledCourses
                .Where(c => QuarterHelper.GetQuarterFromDate(c.StartDate) == QuarterHelper.GetQuarterFromDate(DateTime.Today))
                .ToList();

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
                    int pointsEarned = ag?.PointsEarned ?? 0;
                    int pointsPossible = a.PointsPossible;
                    string grade = GradeCalculator.GetLetterGrade(pointsEarned, pointsPossible);

                    assignmentInfos.Add(new AssignmentInfo
                    {
                        Title = a.Title,
                        DueDate = a.DueDate,
                        PointsPossible = pointsPossible,
                        PointsEarned = pointsEarned,
                        Grade = grade
                    });
                }
                AssignmentsByCourse[course.Id] = assignmentInfos;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUploadProfilePictureAsync(int studentId, IFormFile ProfilePicture)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return NotFound();

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await ProfilePicture.CopyToAsync(ms);
                    student.ProfilePicture = ms.ToArray();
                    student.ProfilePictureContentType = ProfilePicture.ContentType;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { studentId });
        }
    }
}