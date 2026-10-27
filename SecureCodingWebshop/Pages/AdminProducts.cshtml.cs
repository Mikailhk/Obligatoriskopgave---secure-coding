using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecureCodingWebshop.Data;
using SecureCodingWebshop.Models;

namespace SecureCodingWebshop.Pages
{
    public class AdminProductsModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public decimal Price { get; set; }

        private readonly ProductRepository _productRepository;

        public List<Product> Products { get; set; } = new();

        public AdminProductsModel(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Login");
            }

            Products = _productRepository.GetAll();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Login");
            }

            Product product = new Product
            {
                Name = Name,
                Price = Price
            };

            _productRepository.Add(product);

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Login");
            }

            _productRepository.Delete(id);

            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(int id, string name, decimal price)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Login");
            }

            Product product = new Product
            {
                Id = id,
                Name = name,
                Price = price
            };

            _productRepository.Update(product);

            return RedirectToPage();
        }
    }
}