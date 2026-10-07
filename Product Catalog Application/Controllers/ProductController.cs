using Microsoft.AspNetCore.Mvc;
using Product_Catalog_Application.Models;

namespace Product_Catalog_Application.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> _catalog = new List<Product>
        {
            new Product { Id = 1, Name = "Mechanical Keyboard", Category = "Electronics", Price = 79.99m, StockQuantity = 25, Description = "RGB Backlit keyboard" },
            new Product { Id = 2, Name = "Wireless Mouse", Category = "Electronics", Price = 29.50m, StockQuantity = 50, Description = "Ergonomic 2.4GHz wireless mouse" },
            new Product { Id = 3, Name = "USB-C Hub", Category = "Accessories", Price = 19.99m, StockQuantity = 100, Description = "7-in-1 multi-port adapter" }
        };

        public IActionResult Index(string? category)
        {
            var products = string.IsNullOrEmpty(category)
                ? _catalog
                : _catalog.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.SelectedCategory = category;
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _catalog.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = _catalog.Any() ? _catalog.Max(p => p.Id) + 1 : 1;
                _catalog.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _catalog.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _catalog.Remove(product);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}