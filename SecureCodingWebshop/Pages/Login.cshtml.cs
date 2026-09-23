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

        private static int _failedLoginAttempts = 0;
        private static DateTime? _lockoutEnd = null;

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
            if (_lockoutEnd != null && DateTime.Now < _lockoutEnd)
            {
                ModelState.AddModelError("", "Login er midlertidigt blokeret. Prøv igen senere.");
                return Page();
            }

            if (_lockoutEnd != null && DateTime.Now >= _lockoutEnd)
            {
                _failedLoginAttempts = 0;
                _lockoutEnd = null;
            }

            var user = _userRepository.GetUserByEmail(Email);

            if (user == null)
            {
                _failedLoginAttempts++;

                _logService.Log($"Mislykket loginforsøg for email: {Email}");

                ModelState.AddModelError("", "Forkert email eller password");
                return Page();
            }

            bool passwordCorrect =
                _passwordService.VerifyPassword(user, Password);

            if (!passwordCorrect)
            {
                _failedLoginAttempts++;

                if (_failedLoginAttempts >= 3)
                {
                    _lockoutEnd = DateTime.Now.AddMinutes(10);

                    _logService.Log($"Bruger midlertidigt blokeret efter 3 mislykkede loginforsøg: {Email}");

                    ModelState.AddModelError("", "For mange mislykkede loginforsøg.");
                    return Page();
                }

                _logService.Log($"Mislykket loginforsøg for email: {Email}");

                ModelState.AddModelError("", "Forkert email eller password");
                return Page();
            }

            _failedLoginAttempts = 0;

            _logService.Log($"Succesfuldt login for email: {Email}");

            return RedirectToPage("/Index");
        }
    }
}