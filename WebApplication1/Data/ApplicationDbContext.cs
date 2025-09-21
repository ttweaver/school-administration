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
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<Teacher> Teacher { get; set; } = default!;
        public DbSet<Course> Classroom { get; set; } = default!;
        public DbSet<WebApplication1.Models.Score> Score { get; set; } = default!;
    }
}