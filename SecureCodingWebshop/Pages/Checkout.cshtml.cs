using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Models;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;

        public List<CartItem> Items { get; set; } = new();

        public CheckoutModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public void OnGet()
        {
            Items = _cartService.GetItems();
        }

        public void OnPost()
        {
            Message = "Købet er gennemført.";
            Items = _cartService.GetItems();
        }

        public string Message { get; set; } = "";
    }
}