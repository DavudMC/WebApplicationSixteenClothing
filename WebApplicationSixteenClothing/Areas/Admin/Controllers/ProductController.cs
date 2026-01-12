using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplicationSixteenClothing.Contexts;
using WebApplicationSixteenClothing.ViewModels.ProductViewModels;

namespace WebApplicationSixteenClothing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImagePath = x.ImagePath,
                Price = x.Price,
                CategoryName = x.Category.Name,
                Rating = x.Rating
            }).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.Select(c=> new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            return View(vm);
        }
    }
}
