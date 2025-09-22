using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Score
{
    // Define properties as needed, e.g.:
    public int Value { get; set; }
    public string Name { get; set; }
}

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<Student> Students { get; set; } = new List<Student>();
        public List<Score> Score { get; set; } = new List<Score>();

        public void OnGet()
        {

        }
    }
}
