using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;
        private readonly LogService _logService;

        public LoginModel(
            UserRepository userRepository,
            PasswordService passwordService,
            LogService logService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logService = logService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _userRepository.GetUserByEmail(Email);

            if (user == null)
            {
                _logService.Log($"Mislykket loginforsøg for email: {Email}");

                ModelState.AddModelError("", "Forkert email eller password");
                return Page();
            }

            bool passwordCorrect =
                _passwordService.VerifyPassword(user, Password);

            if (!passwordCorrect)
            {
                _logService.Log($"Mislykket loginforsøg for email: {Email}");

                ModelState.AddModelError("", "Forkert email eller password");
                return Page();
            }

            _logService.Log($"Succesfuldt login for email: {Email}");

            return RedirectToPage("/Index");
        }
    }
}