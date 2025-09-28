using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;



namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Teacher> Teachers { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
		public DbSet<Assignment> Assignments { get; set; } = default!;
		public DbSet<AssignmentScore> AssignmentScores { get; set; } = default!;
        public DbSet<Enrollment> Enrollments { get; set; } = default!;
        public object Course { get; internal set; }
    }
}