using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Models;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly OrderRepository _orderRepository;

        public List<CartItem> Items { get; set; } = new();

        public string Message { get; set; } = "";

        public CheckoutModel(CartService cartService, OrderRepository orderRepository)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
        }

        public void OnGet()
        {
            Items = _cartService.GetItems();
        }

        public IActionResult OnPost()
        {
            string? email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Login");
            }

            Items = _cartService.GetItems();

            if (Items.Count == 0)
            {
                Message = "Kurven er tom.";
                return Page();
            }

            decimal totalPrice = Items.Sum(item => item.Price * item.Quantity);

            _orderRepository.AddOrder(email, totalPrice);

            _cartService.ClearCart();
            Items = _cartService.GetItems();

            Message = "Købet er gennemført.";

            return Page();
        }
    }
}
