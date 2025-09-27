using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public static class GradeCalculator
    {
        public static (double Earned, double Possible) CalculateStudentGrade(
            int studentId,
            List<Assignment> assignments)
        {
            // Only assignments with at least one graded score
            var gradedAssignments = assignments
                .Where(a => a.AssignmentScores != null && a.AssignmentScores.Any(s => s.IsGraded))
                .ToList();

            var possiblePoints = gradedAssignments.Sum(a => (double)a.PointsPossible);

            var studentScores = gradedAssignments
                .SelectMany(a => a.AssignmentScores
                    .Where(s => s.StudentId == studentId && s.IsGraded && s.PointsEarned.HasValue))
                .ToList();

            var earnedPoints = studentScores.Sum(s => s.PointsEarned.Value);

            // If all PointsEarned are null, do not assign a grade
            if (studentScores.Count == 0)
            {
                return (0, 0);
            }
            else
            {
                return (earnedPoints, possiblePoints);
            }
        }

        public static string GetLetterGrade(double earned, double possible)
        {
            if (possible == 0 || earned == 0) return "-";
            var percent = earned / possible * 100;
            return percent switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };
        }
    }
}