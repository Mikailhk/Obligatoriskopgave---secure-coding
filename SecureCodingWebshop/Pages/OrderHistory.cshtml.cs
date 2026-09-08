using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Models;

namespace SecureCodingWebshop.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly OrderRepository _orderRepository;

        public List<Order> Orders { get; set; } = new();

        public OrderHistoryModel(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void OnGet(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            { 
                Orders = _orderRepository.GetOrdersByEmail(email); 
            }
        }
    }
}