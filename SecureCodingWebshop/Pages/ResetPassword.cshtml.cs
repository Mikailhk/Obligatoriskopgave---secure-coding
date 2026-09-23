using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserRepository _userRepository;

        private readonly PasswordService _passwordService;

        public ResetPasswordModel(
            UserRepository userRepository,
            PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public void OnGet(string token)
        {
            Token = token;
        }

        public IActionResult OnPost()
        {
            var user = _userRepository.GetUserByResetToken(Token);

            if (user == null)
            {
                ModelState.AddModelError("", "Reset-linket er ugyldigt eller udløbet.");
                return Page();
            }

            string newPasswordHash = _passwordService.HashPassword(user, NewPassword);

            _userRepository.ResetPassword(user.Id, newPasswordHash);

            return RedirectToPage("/Login");
        }
    }
}
