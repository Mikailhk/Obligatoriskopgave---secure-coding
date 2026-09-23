using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;

namespace SecureCodingWebshop.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserRepository _userRepository;

        public ForgotPasswordModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public string ResetLink { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _userRepository.GetUserByEmail(Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bruger blev ikke fundet.");
                return Page();
            }

            string token = Guid.NewGuid().ToString();
            DateTime tokenExpiry = DateTime.Now.AddMinutes(15);
            _userRepository.SaveResetToken(Email, token, tokenExpiry);

            ResetLink = $"/ResetPassword?token={token}";

            Message = "Der er oprettet et link til nulstilling af password.";

            return Page();
        }
    }
}
