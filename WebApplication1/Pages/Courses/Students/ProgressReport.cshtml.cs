using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication1.Pages.Courses.Students
{
    public class ProgressReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProgressReportModel(ApplicationDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }

        public async Task<IActionResult> OnGetAsync(int studentId, int courseId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                return NotFound();

            var assignments = await _context.Assignments
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            var scores = await _context.AssignmentScores
                .Where(s => s.StudentId == studentId && assignments.Select(a => a.Id).Contains(s.AssignmentId))
                .Include(s => s.Assignment)
                .ToListAsync();

            var pdf = GeneratePdf(student, assignments, scores);

            return File(pdf, "application/pdf", $"ProgressReport_{student.LastName}_Course{courseId}.pdf");
        }

        private byte[] GeneratePdf(Student student, IList<Assignment> assignments, IList<AssignmentScore> scores)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Header().Text($"Progress Report: {student.FirstName} {student.LastName}").FontSize(20).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(88);
                            columns.ConstantColumn(88);
                        });

                        table.Header(header =>
                        {
                            header.Cell().PaddingTop(10).PaddingBottom(10).Text("Assignment Title").Bold();
                            header.Cell().PaddingTop(10).PaddingBottom(10).Text("Points Possible").Bold();
                            header.Cell().PaddingTop(10).PaddingBottom(10).Text("Points Earned").Bold();
                        });

                        float paragraphSpacing = 10;
						foreach (var assignment in assignments)
                        {
                            var score = scores.FirstOrDefault(s => s.AssignmentId == assignment.Id);
                            table.Cell().PaddingBottom(paragraphSpacing).Text(assignment.Title);
                            table.Cell().PaddingBottom(paragraphSpacing).Text(assignment.PointsPossible.ToString());
                            table.Cell().PaddingBottom(paragraphSpacing).Text(score?.PointsEarned?.ToString() ?? "-");
                        }
                    });
                });
            }).GeneratePdf();
        }
    }
}