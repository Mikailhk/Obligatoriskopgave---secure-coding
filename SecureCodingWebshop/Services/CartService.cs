using SecureCodingWebshop.Models;

namespace SecureCodingWebshop.Services
{
    public class CartService
    {
        private readonly List<CartItem> _items = new();

        public void AddToCart(Product product)
        {
            _items.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            });
        }

        public List<CartItem> GetItems()
        {
            return _items;
        }
    }
}