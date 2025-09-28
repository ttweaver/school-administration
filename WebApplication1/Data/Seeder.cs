using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebApplication1.Models;
using System.Drawing;

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
                // List of top five email domains
                string[] emailDomains = new[]
                {
                    "gmail.com",
                    "yahoo.com",
                    "outlook.com",
                    "icloud.com",
                    "hotmail.com"
                };
                string[] countries = new[] { "USA", "Canada", "UK", "Australia", "Germany" };
                string[] states = new[] { "NY", "CA", "TX", "FL", "IL", "PA", "OH", "GA", "NC", "MI" };
                string[] cities = new[] { "New York", "Los Angeles", "Chicago", "Houston", "Miami", "Dallas", "Atlanta", "Philadelphia", "Phoenix", "San Antonio" };
                string[] streetNames = new[] { "Main St", "Maple Ave", "Oak St", "Pine St", "Cedar Ave", "Elm St", "Washington Ave", "Lake St", "Hill St", "Sunset Blvd" };
                string[] emergencyNames = new[] { "Pat Smith", "Chris Johnson", "Taylor Brown", "Morgan Lee", "Casey Davis", "Jamie Miller", "Riley Martinez", "Avery Hernandez", "Peyton Lopez", "Drew Gonzalez" };

                // Sprite sheet for avatars
                string spriteSheetPath = @".\wwwroot\images\avatars.jpg"; // The single image file
                int columns = 8;
                int rows = 4;
                int avatarWidth = 240;  // Set to your avatar width
                int avatarHeight = 240; // Set to your avatar height

                List<byte[]> avatarImages = new List<byte[]>();
                List<string> avatarContentTypes = new List<string>();

                if (File.Exists(spriteSheetPath))
                {
                    using (var spriteSheet = new Bitmap(spriteSheetPath))
                    {
                        int offsetX = 55; // Change if your grid starts away from the left edge
                        int offsetY = 30; // Change if your grid starts away from the top edge
                        int spacingX = -16; // Change if there is horizontal spacing between sprites
                        int spacingY = -20; // Change if there is vertical spacing between sprites

                        for (int row = 0; row < rows; row++)
                        {
                            for (int col = 0; col < columns; col++)
                            {
                                int x = offsetX + col * (avatarWidth + spacingX);
                                int y = offsetY + row * (avatarHeight + spacingY);
                                using (var avatarBmp = new Bitmap(avatarWidth, avatarHeight))
                                using (var g = Graphics.FromImage(avatarBmp))
                                {
                                    g.DrawImage(spriteSheet, new Rectangle(0, 0, avatarWidth, avatarHeight),
                                        new Rectangle(x, y, avatarWidth, avatarHeight),
                                        GraphicsUnit.Pixel);

                                    // Save to disk for debugging
                                    //avatarBmp.Save($"avatar_{row}_{col}.png", System.Drawing.Imaging.ImageFormat.Png);

                                    using (var ms = new MemoryStream())
                                    {
                                        avatarBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                        avatarImages.Add(ms.ToArray());
                                        avatarContentTypes.Add("image/png");
                                    }
                                }
                            }
                        }
                    }
                }

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
                        var domain = emailDomains[rand.Next(emailDomains.Length)];
                        var country = countries[rand.Next(countries.Length)];
                        var state = states[rand.Next(states.Length)];
                        var city = cities[rand.Next(cities.Length)];
                        var streetNumber = rand.Next(100, 999);
                        var street = streetNames[rand.Next(streetNames.Length)];
                        var postalCode = $"{rand.Next(10000, 99999)}";
                        var mobilePhone = $"{rand.Next(200, 999)}-{rand.Next(200, 999)}-{rand.Next(1000, 9999)}";
                        var homePhone = $"{rand.Next(200, 999)}-{rand.Next(200, 999)}-{rand.Next(1000, 9999)}";
                        var emergencyContactName = emergencyNames[rand.Next(emergencyNames.Length)];
                        var emergencyContactPhone = $"{rand.Next(200, 999)}-{rand.Next(200, 999)}-{rand.Next(1000, 9999)}";

                        byte[]? profilePicture = null;
                        string? profilePictureContentType = null;
                        if (avatarImages.Count > 0)
                        {
                            int avatarIndex = (i - 1) % avatarImages.Count;
                            profilePicture = avatarImages[avatarIndex];
                            profilePictureContentType = avatarContentTypes[avatarIndex];
                        }

                        return new Student
                        {
                            FirstName = first,
                            LastName = last,
                            Email = $"{first}.{last}{num}@{domain}".ToLower(),
                            DateOfBirth = new DateTime(
                                rand.Next(2000, 2011),
                                rand.Next(1, 13),
                                rand.Next(1, 29)
                            ),
                            StreetAddress = $"{streetNumber} {street}",
                            City = city,
                            State = state,
                            PostalCode = postalCode,
                            Country = country,
                            MobilePhone = mobilePhone,
                            HomePhone = homePhone,
                            EmergencyContactName = emergencyContactName,
                            EmergencyContactPhone = emergencyContactPhone,
                            ProfilePicture = profilePicture,
                            ProfilePictureContentType = profilePictureContentType
                        };
                    }).ToList();
                context.Students.AddRange(students);

                // List of all US area codes (sample, not exhaustive; for full list, expand as needed)
                string[] usAreaCodes = new[]
                {
                    "201","202","203","205","206","207","208","209","210","212","213","214","215","216","217","218","219","224","225","228","229",
                    "231","234","239","240","248","251","252","253","254","256","260","262","267","269","270","272","274","276","281","301","302",
                    "303","304","305","307","308","309","310","312","313","314","315","316","317","318","319","320","321","323","325","327","330",
                    "331","334","336","337","339","346","351","352","360","361","364","380","385","386","401","402","404","405","406","407","408",
                    "409","410","412","413","414","415","417","419","423","424","425","430","432","434","435","440","442","443","445","464","469",
                    "470","475","478","479","480","484","501","502","503","504","505","507","508","509","510","512","513","515","516","517","518",
                    "520","530","531","534","539","540","541","551","559","561","562","563","564","567","570","571","573","574","575","580","582",
                    "585","586","601","602","603","605","606","607","608","609","610","612","614","615","616","617","618","619","620","623","626",
                    "628","629","630","631","636","641","646","650","651","657","659","660","661","662","667","669","678","680","681","682","701",
                    "702","703","704","706","707","708","712","713","714","715","716","717","718","719","720","724","725","727","730","731","732",
                    "734","737","740","743","747","754","757","758","760","762","763","765","769","770","772","773","774","775","779","781","784",
                    "785","786","801","802","803","804","805","806","808","810","812","813","814","815","816","817","818","828","830","831","832",
                    "838","839","840","843","845","847","848","850","854","856","857","858","859","860","862","863","864","865","870","872","878",
                    "901","903","904","906","907","908","909","910","912","913","914","915","916","917","918","919","920","925","928","929","930",
                    "931","934","936","937","938","940","941","945","947","949","951","952","954","956","959","970","971","972","973","975","978",
                    "979","980","984","985","986","989"
                };

                // List of 50 different US cities and states
                var usCitiesAndStates = new[]
                {
                    new { City = "New York", State = "NY" },
                    new { City = "Los Angeles", State = "CA" },
                    new { City = "Chicago", State = "IL" },
                    new { City = "Houston", State = "TX" },
                    new { City = "Phoenix", State = "AZ" },
                    new { City = "Philadelphia", State = "PA" },
                    new { City = "San Antonio", State = "TX" },
                    new { City = "San Diego", State = "CA" },
                    new { City = "Dallas", State = "TX" },
                    new { City = "San Jose", State = "CA" },
                    new { City = "Austin", State = "TX" },
                    new { City = "Jacksonville", State = "FL" },
                    new { City = "Fort Worth", State = "TX" },
                    new { City = "Columbus", State = "OH" },
                    new { City = "Charlotte", State = "NC" },
                    new { City = "San Francisco", State = "CA" },
                    new { City = "Indianapolis", State = "IN" },
                    new { City = "Seattle", State = "WA" },
                    new { City = "Denver", State = "CO" },
                    new { City = "Washington", State = "DC" },
                    new { City = "Boston", State = "MA" },
                    new { City = "El Paso", State = "TX" },
                    new { City = "Nashville", State = "TN" },
                    new { City = "Detroit", State = "MI" },
                    new { City = "Oklahoma City", State = "OK" },
                    new { City = "Portland", State = "OR" },
                    new { City = "Las Vegas", State = "NV" },
                    new { City = "Memphis", State = "TN" },
                    new { City = "Louisville", State = "KY" },
                    new { City = "Baltimore", State = "MD" },
                    new { City = "Milwaukee", State = "WI" },
                    new { City = "Albuquerque", State = "NM" },
                    new { City = "Tucson", State = "AZ" },
                    new { City = "Fresno", State = "CA" },
                    new { City = "Mesa", State = "AZ" },
                    new { City = "Sacramento", State = "CA" },
                    new { City = "Atlanta", State = "GA" },
                    new { City = "Kansas City", State = "MO" },
                    new { City = "Colorado Springs", State = "CO" },
                    new { City = "Miami", State = "FL" },
                    new { City = "Raleigh", State = "NC" },
                    new { City = "Omaha", State = "NE" },
                    new { City = "Long Beach", State = "CA" },
                    new { City = "Virginia Beach", State = "VA" },
                    new { City = "Oakland", State = "CA" },
                    new { City = "Minneapolis", State = "MN" },
                    new { City = "Tulsa", State = "OK" },
                    new { City = "Arlington", State = "TX" },
                    new { City = "Tampa", State = "FL" },
                    new { City = "New Orleans", State = "LA" }
                };

                // List of sample US street names
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
                        var domain = emailDomains[rand.Next(emailDomains.Length)];
                        var areaCode = usAreaCodes[rand.Next(usAreaCodes.Length)];
                        var phone = $"{areaCode}-{rand.Next(200, 1000):D3}-{rand.Next(1000, 10000):D4}";
                        var cityState = usCitiesAndStates[rand.Next(usCitiesAndStates.Length)];
                        var streetNumber = rand.Next(100, 1000); // 3-digit number
                        var streetName = streetNames[rand.Next(streetNames.Length)];
                        return new Teacher
                        {
                            FirstName = first,
                            LastName = last,
                            Email = $"{first}.{last}{num}@{domain}".ToLower(),
                            Qualifications = qual,
                            Contact = phone,
                            Address = $"{streetNumber} {streetName}, {cityState.City}, {cityState.State}",
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

                // Get today's date
                var today = DateTime.Today;

                // Find the current quarter (the first quarter whose Start <= today and End >= today)
                var currentQuarter = quarters.FirstOrDefault(q => q.Start <= today && q.End >= today);

                // Add 10 assignments of different types per course
                var assignmentTypes = Enum.GetValues(typeof(AssignmentType)).Cast<AssignmentType>().ToArray();
                foreach (var course in courses)
                {
                    if (course.StartDate == null || course.EndDate == null) continue;
                    var start = course.StartDate.Value;
                    var end = course.EndDate.Value;
                    int assignmentCount = 10;
                    var totalDays = (end - start).TotalDays;
                    var interval = totalDays / (assignmentCount + 1);

                    for (int j = 0; j < assignmentCount; j++)
                    {
                        var randomType = assignmentTypes[rand.Next(assignmentTypes.Length)];
                        var dateAssigned = start.AddDays(interval * (j + 1));
                        var dueDate = dateAssigned.AddDays(7);

                        var assignment = new Assignment
                        {
                            Title = $"{randomType} for {course.Name}",
                            Type = randomType,
                            DateAssigned = dateAssigned,
                            DueDate = dueDate,
                            Course = course,
                            Description = $"This is the {randomType} for {course.Name}.",
                        };
                        context.Assignments.Add(assignment);

                        // Only score assignments in the current quarter and up to today
                        if (currentQuarter != null &&
                            dateAssigned >= currentQuarter.Start &&
                            dateAssigned <= today)
                        {
                            foreach (var enrollment in context.Enrollments.Local.Where(e => e.Course == course).ToList())
                            {
                                int maxPoints = assignment.PointsPossible;
                                int pointsEarned = rand.Next(maxPoints / 2, maxPoints + 1);

                                var assignmentScore = new AssignmentScore
                                {
                                    Assignment = assignment,
                                    Student = enrollment.Student,
                                    PointsEarned = pointsEarned
                                };
                                context.AssignmentScores.Add(assignmentScore);
                            }
                        }
                    }
                }

                context.SaveChanges();
            }
        }
    }
}