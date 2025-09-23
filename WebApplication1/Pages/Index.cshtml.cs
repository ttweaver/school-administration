using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

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
        public List<AssignmentScore> Score { get; set; } = new List<AssignmentScore>();

        public void OnGet()
        {

        }
    }
}
