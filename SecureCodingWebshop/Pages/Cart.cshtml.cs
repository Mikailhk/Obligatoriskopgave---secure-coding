using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Models;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class CartModel : PageModel
    {
        private readonly CartService _cartService;

        public List<CartItem> Items { get; set; } = new();

        public CartModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public void OnGet()
        {
            Items = _cartService.GetItems();
        }
    }
}
