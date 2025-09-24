using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public static class Seeder
    {
        public static void Seed(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            // Seed Identity roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new[]
            {
                "UserManager",
                "Admin",
                "Teacher",
                "StudentModify",
                "Student",
                "TeacherModify",
                "CourseModify"
            };
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }

            if (!context.Students.Any())
            {
                // Add 100 students
                // List of sample last names
                string[] lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee" };
                string[] firstNames = new[] { "Alex", "Jordan", "Taylor", "Morgan", "Casey", "Jamie", "Riley", "Avery", "Peyton", "Drew", "Skyler", "Cameron", "Reese", "Quinn", "Harper", "Rowan", "Sawyer", "Emerson", "Finley", "Hayden" };
                var rand = new Random();
                var students = Enumerable.Range(1, 100)
                    .Select(i =>
                    {
                        var first = firstNames[rand.Next(firstNames.Length)];
                        var last = lastNames[rand.Next(lastNames.Length)];
                        var num = rand.Next(1, 101);
                        return new Student
                        {
                            FirstName = first,
                            LastName = last,
                            Email = $"{first}.{last}{num}@example.com".ToLower(),
                            DateOfBirth = new DateTime(
                                rand.Next(2000, 2011),
                                rand.Next(1, 13),
                                rand.Next(1, 29)
                            )
                        };
                    }).ToList();
                context.Students.AddRange(students);

                // Add 50 teachers
                var teachers = Enumerable.Range(1, 50)
                    .Select(i => new Teacher
                    {
                        FirstName = $"Teacher{i}",
                        LastName = $"TeachLast{i}",
                        Email = $"teacher{i}@example.com",
                        Qualifications = "M.Ed",
                        Contact = "123-456-7890",
                        Address = $"123 Teacher St, City{i}, State{i}",
                        ClassesTaught = $"Course {i}"
					}).ToList();
                context.Teachers.AddRange(teachers);

                // Add 50 courses, each with a teacher
                var courses = Enumerable.Range(1, 50)
                    .Select(i => new Course
                    {
                        Name = $"Course {i}",
                        Description = $"Description for Course {i}",
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddMonths(4),
                        Teacher = teachers[i - 1],
                        TeacherId = teachers[i - 1].Id
					}).ToList();
                context.Courses.AddRange(courses);

                // Enroll each student in 5 random courses
                rand = new Random();
                foreach (var student in students)
                {
                    var courseIds = courses.OrderBy(_ => rand.Next()).Take(5).ToList();
                    foreach (var course in courseIds)
                    {
                        context.Enrollments.Add(new Enrollment
                        {
                            Student = student,
                            Course = course
                        });
                    }
                }

                // Add 10 assignments of different types per course
                foreach (var course in courses)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        var assignment = new Assignment
                        {
                            Title = $"{AssignmentType.Assignment} for {course.Name}",
                            Type = AssignmentType.Assignment,
							DueDate = DateTime.Today.AddDays(7 + j),
                            PointsPossible = 100,
                            Course = course,
                            Description = $"This is the {AssignmentType.Assignment} for {course.Name}.",
						};
                        context.Assignments.Add(assignment);
                    }
                }

                context.SaveChanges();
            }
        }
    }
}