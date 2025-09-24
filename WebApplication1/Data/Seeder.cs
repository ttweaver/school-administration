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
                string[] studentLastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee" };
                string[] studentFirstNames = new[] { "Alex", "Jordan", "Taylor", "Morgan", "Casey", "Jamie", "Riley", "Avery", "Peyton", "Drew", "Skyler", "Cameron", "Reese", "Quinn", "Harper", "Rowan", "Sawyer", "Emerson", "Finley", "Hayden" };
                var rand = new Random();
                var students = Enumerable.Range(1, 100)
                    .Select(i =>
                    {
                        var first = studentFirstNames[rand.Next(studentFirstNames.Length)];
                        var last = studentLastNames[rand.Next(studentLastNames.Length)];
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

                // Add 50 teachers with a different set of names and random qualifications
                string[] teacherFirstNames = new[] { "Evelyn", "Mason", "Logan", "Harold", "Vivian", "Stella", "Miles", "Clara", "Oscar", "Nora", "Silas", "Hazel", "Jasper", "Ivy", "Otis", "Mabel", "Felix", "Pearl", "Hugo", "June" };
                string[] teacherLastNames = new[] { "Bennett", "Foster", "Griffin", "Hayes", "Jenkins", "Keller", "Lawson", "Manning", "Nash", "Owens", "Parker", "Quinn", "Ramsey", "Sawyer", "Turner", "Underwood", "Vaughn", "Walker", "Young", "Zimmerman" };
                string[] qualifications = new[] { "BS", "MS", "PhD" };
                var teachers = Enumerable.Range(1, 50)
                    .Select(i =>
                    {
                        var first = teacherFirstNames[rand.Next(teacherFirstNames.Length)];
                        var last = teacherLastNames[rand.Next(teacherLastNames.Length)];
                        var num = rand.Next(1, 101);
                        var qual = qualifications[rand.Next(qualifications.Length)];
                        return new Teacher
                        {
                            FirstName = first,
                            LastName = last,
                            Email = $"{first}.{last}{num}@example.com".ToLower(),
                            Qualifications = qual,
                            Contact = "123-456-7890",
                            Address = $"123 Teacher St, City{i}, State{i}",
                            ClassesTaught = $"Course {i}"
                        };
                    }).ToList();
                context.Teachers.AddRange(teachers);

                // List of 50 real or realistic course names and descriptions for 2025
                var courseCatalog = new[]
                {
                    new { Name = "CS101 Introduction to Computer Science", Description = "Fundamental concepts of computer science, including algorithms, programming, and problem-solving." },
                    new { Name = "CS120 Programming with Python", Description = "Introduction to programming using Python, covering syntax, data structures, and basic algorithms." },
                    new { Name = "CS200 Data Structures and Algorithms", Description = "Study of data structures, algorithm analysis, and their applications in software development." },
                    new { Name = "CS250 Web Development Fundamentals", Description = "Covers HTML, CSS, JavaScript, and the basics of building interactive web applications." },
                    new { Name = "CS300 Database Systems", Description = "Introduction to relational databases, SQL, and data modeling concepts." },
                    new { Name = "CS320 Software Engineering", Description = "Principles of software development, project management, and software lifecycle." },
                    new { Name = "CS350 Operating Systems", Description = "Explores operating system concepts, including processes, memory management, and file systems." },
                    new { Name = "CS370 Computer Networks", Description = "Covers networking fundamentals, protocols, and network security basics." },
                    new { Name = "CS400 Cybersecurity Fundamentals", Description = "Introduction to cybersecurity principles, threats, and defense mechanisms." },
                    new { Name = "CS420 Artificial Intelligence", Description = "Overview of AI concepts, machine learning, and intelligent systems." },
                    new { Name = "CS430 Cloud Computing", Description = "Covers cloud service models, deployment, and management in modern IT." },
                    new { Name = "CS440 Mobile Application Development", Description = "Design and development of mobile applications for Android and iOS." },
                    new { Name = "CS450 Advanced Java Programming", Description = "In-depth Java programming, including OOP, GUIs, and concurrency." },
                    new { Name = "CS460 C# and .NET Development", Description = "Building applications using C# and the .NET platform." },
                    new { Name = "CS470 Software Testing and Quality Assurance", Description = "Principles and practices of software testing and QA." },
                    new { Name = "CS480 Game Development Fundamentals", Description = "Introduction to game design and development using modern engines." },
                    new { Name = "CS490 Capstone Project", Description = "A culminating project integrating knowledge from the computer science curriculum." },
                    new { Name = "IT101 Introduction to Information Technology", Description = "Overview of IT concepts, hardware, software, and careers." },
                    new { Name = "IT120 Networking Essentials", Description = "Basic networking concepts, topologies, and protocols." },
                    new { Name = "IT200 Linux Administration", Description = "Linux OS installation, configuration, and administration." },
                    new { Name = "IT210 Windows Server Administration", Description = "Managing and configuring Windows Server environments." },
                    new { Name = "IT220 Virtualization Technologies", Description = "Principles and practices of virtualization in IT." },
                    new { Name = "IT230 IT Security Fundamentals", Description = "Core concepts in IT security, risk management, and compliance." },
                    new { Name = "IT240 Scripting for Administrators", Description = "Automating tasks using PowerShell and Bash scripting." },
                    new { Name = "IT250 Cloud Infrastructure", Description = "Design and management of cloud-based IT infrastructure." },
                    new { Name = "IT260 IT Project Management", Description = "Project management methodologies and tools for IT projects." },
                    new { Name = "IT270 Disaster Recovery and Business Continuity", Description = "Strategies for IT disaster recovery and continuity planning." },
                    new { Name = "IT280 Wireless Networking", Description = "Wireless network design, security, and troubleshooting." },
                    new { Name = "IT290 IT Support and Help Desk", Description = "Best practices for IT support and customer service." },
                    new { Name = "MATH101 College Algebra", Description = "Algebraic concepts and problem-solving for college students." },
                    new { Name = "MATH201 Discrete Mathematics", Description = "Logic, set theory, combinatorics, and graph theory for CS." },
                    new { Name = "MATH210 Calculus I", Description = "Differential and integral calculus with applications." },
                    new { Name = "MATH220 Statistics for Computing", Description = "Statistical methods and analysis for IT and CS." },
                    new { Name = "ENG101 English Composition", Description = "Fundamentals of academic writing and research." },
                    new { Name = "ENG201 Technical Writing", Description = "Writing technical documents and communication in IT." },
                    new { Name = "BUS101 Introduction to Business", Description = "Business concepts, structures, and environments." },
                    new { Name = "BUS201 Business Ethics", Description = "Ethical issues and decision-making in business." },
                    new { Name = "BUS210 Project Management Fundamentals", Description = "Project management principles and practices." },
                    new { Name = "BUS220 Entrepreneurship", Description = "Starting and managing new business ventures." },
                    new { Name = "SCI101 Environmental Science", Description = "Principles of ecology, sustainability, and the environment." },
                    new { Name = "SCI201 Physics for Computing", Description = "Physics concepts relevant to computing and engineering." },
                    new { Name = "SCI210 Introduction to Biology", Description = "Fundamentals of biology for non-majors." },
                    new { Name = "HUM101 Introduction to Humanities", Description = "Exploration of human culture, art, and philosophy." },
                    new { Name = "HUM201 Ethics in Technology", Description = "Ethical considerations in technology and computing." },
                    new { Name = "SOC101 Introduction to Sociology", Description = "Study of society, social institutions, and relationships." },
                    new { Name = "PSY101 Introduction to Psychology", Description = "Principles of psychology and human behavior." },
                    new { Name = "HIS101 U.S. History", Description = "Survey of U.S. history from colonial times to present." },
                    new { Name = "HIS201 World History", Description = "Overview of major events in world history." },
                    new { Name = "COM101 Public Speaking", Description = "Principles and practice of effective oral communication." }
                };

                // Define quarter start/end dates for 2025
                var quarters = new[]
                {
                    new { Name = "Winter", Start = new DateTime(2025, 1, 6), End = new DateTime(2025, 3, 21) },
                    new { Name = "Spring", Start = new DateTime(2025, 3, 31), End = new DateTime(2025, 6, 13) },
                    new { Name = "Summer", Start = new DateTime(2025, 6, 23), End = new DateTime(2025, 9, 5) },
                    new { Name = "Fall",   Start = new DateTime(2025, 9, 15), End = new DateTime(2025, 11, 28) }
                };

                // Add 50 courses, cycling through the course catalog and quarters
                var courses = Enumerable.Range(1, 50)
                    .Select(i => {
                        var courseInfo = courseCatalog[(i - 1) % courseCatalog.Length];
                        var quarter = quarters[(i - 1) % quarters.Length];
                        return new Course
                        {
                            Name = courseInfo.Name,
                            Description = courseInfo.Description,
                            StartDate = quarter.Start,
                            EndDate = quarter.End,
                            Teacher = teachers[i - 1],
                            TeacherId = teachers[i - 1].Id
                        };
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