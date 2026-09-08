using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public ChangePasswordModel(
            UserRepository userRepository,
            PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            var user = _userRepository.GetUserByEmail(Email);

            if (user == null)
            {
                return;
            }

            bool currentPasswordCorrect =
                _passwordService.VerifyPassword(user, CurrentPassword);

            if (!currentPasswordCorrect)
            {
                return;
            }

            string newHash =
                _passwordService.HashPassword(user, NewPassword);

            _userRepository.UpdatePassword(user.Id, newHash);
        }
    }
}