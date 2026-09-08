using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecureCodingWebshop.Pages
{
    public class AdminLogsModel : PageModel
    {
        public List<string> Logs { get; set; } = new();

        public void OnGet()
        {
            if (System.IO.File.Exists("logs.txt"))
            {
                Logs = System.IO.File.ReadAllLines("logs.txt").ToList();
            }
        }
    }
}