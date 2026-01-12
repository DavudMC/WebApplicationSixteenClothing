using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplicationSixteenClothing.Contexts;
using WebApplicationSixteenClothing.ViewModels.ProductViewModels;


namespace WebApplicationSixteenClothing.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _context.Products.Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImagePath = x.ImagePath,
                CategoryName = x.Category.Name,
                Price = x.Price,
                Rating = x.Rating
            }).ToListAsync();
            return View(products);
        }
    }
}
