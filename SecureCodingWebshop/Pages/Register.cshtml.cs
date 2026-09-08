using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Models;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public RegisterModel(
            UserRepository userRepository,
            PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
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
            if (Password.Length < 8)
            {
                ModelState.AddModelError("", "Password skal være mindst 8 tegn.");
                return Page();
            }

            User user = new User
            {
                Email = Email
            };

            user.PasswordHash = _passwordService.HashPassword(user, Password);

            _userRepository.CreateUser(user);

            return RedirectToPage("/Login");
        }
    }
}