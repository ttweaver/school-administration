namespace WebApplication1.Models

{
    public class Teacher
    {
        public int Id { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Qualifications { get; set; }
        public required string Email { get; set; }
        public string? ClassesTaught { get; set; }
    }
}
