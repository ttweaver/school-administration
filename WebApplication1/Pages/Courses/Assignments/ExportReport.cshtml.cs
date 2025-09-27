using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication1.Pages.Scores
{
    public class ExportReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ExportReportModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int studentId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            var scores = await _context.AssignmentScores
                .Include(s => s.Assignment.Course)
                .Where(s => s.StudentId == studentId)
                .ToListAsync();

            if (student == null)
                return NotFound();

            var pdf = GeneratePdf(student, (IList<AssignmentScore>)scores);

            return File(pdf, "application/pdf", $"ProgressReport_{student.LastName}.pdf");
        }

        private byte[] GeneratePdf(Student student, IList<AssignmentScore> scores)
        {
            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Header().Text($"Progress Report: {student.LastName}").FontSize(20).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100);
                            columns.RelativeColumn();
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Course");
                            header.Cell().Text("Title");
                            header.Cell().Text("Earned");
                            header.Cell().Text("Possible");
                        });

                        foreach (var score in scores)
                        {
                            table.Cell().Text(score.Assignment?.Title);
                            table.Cell().Text(""); // No Title property, so leave blank or replace as needed
                            table.Cell().Text(score.PointsEarned?.ToString("F2"));
                            table.Cell().Text(""); // No PointsPossible property, so leave blank or replace as needed
                        }
                    });
                });
            }).GeneratePdf();
        }
    }
}