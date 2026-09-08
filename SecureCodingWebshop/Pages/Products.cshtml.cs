using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Models;
using SecureCodingWebshop.Services;

namespace SecureCodingWebshop.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ProductRepository _productRepository;
        private readonly CartService _cartService;

        public List<Product> Products { get; set; } = new();

        public ProductsModel(ProductRepository productRepository, CartService cartService)
        {
            _productRepository = productRepository;
            _cartService = cartService;
        }
        public void OnGet(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                Products = _productRepository.GetAll();
            }
            else
            {
                Products = _productRepository.Search(search);
            }
        }

        public IActionResult OnPost(int productId)
        {
            var product = _productRepository.GetById(productId);

            if (product != null)
            {
                _cartService.AddToCart(product);
            }

            return RedirectToPage();
        }
    }
}
