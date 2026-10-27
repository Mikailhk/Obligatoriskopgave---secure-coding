
using System.Text.Json;
using SecureCodingWebshop.Models;

namespace SecureCodingWebshop.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(Product product)
        {
            var items = GetItems();

            items.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            });

            SaveItems(items);
        }

        public List<CartItem> GetItems()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            string? json = session.GetString("Cart");

            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new();
        }

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext!.Session.Remove("Cart");
        }

        private void SaveItems(List<CartItem> items)
        {
            string json = JsonSerializer.Serialize(items);
            _httpContextAccessor.HttpContext!.Session.SetString("Cart", json);
        }
    }
}
