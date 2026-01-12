using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplicationSixteenClothing.Contexts;
using WebApplicationSixteenClothing.Helpers;
using WebApplicationSixteenClothing.Models;
using WebApplicationSixteenClothing.ViewModels.ProductViewModels;

namespace WebApplicationSixteenClothing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;
        public ProductController(IWebHostEnvironment environment,AppDbContext dbContext)
        {
            _environment = environment;
            _context = dbContext;
            _folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
        }
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
            await _SendCategoriesWithViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory) 
            {
                ModelState.AddModelError("CategoryId", "Bele bir category movcud deyil!");
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            if(!vm.Image.CheckType("image"))
            {
                ModelState.AddModelError("image", "Zehmet olmasa image tipinde data daxil edin!");
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            if (!vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("image", "Zehmet olmasa max 2mb-liq image tipinde data daxil edin!");
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            string uniquefileName = await vm.Image.FileUploadAsync(_folderPath);
            Product product = new()
            {
                Name = vm.Name,
                Price = vm.Price,
                Description = vm.Description,
                Rating = vm.Rating,
                CategoryId = vm.CategoryId,
                ImagePath = uniquefileName
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var isexistProduct = await _context.Products.FindAsync(id);
            if (isexistProduct == null)
            {
                return NotFound();
            }
            _context.Products.Remove(isexistProduct);
            await _context.SaveChangesAsync();
            string deletedImagePath = Path.Combine(_folderPath,isexistProduct.ImagePath);
            ExtensionMethods.FileDelete(deletedImagePath);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            ProductUpdateVM updateVM = new ProductUpdateVM()
            {
                Id = product.Id,
                Name = product.Name,    
                Description = product.Description,
                Rating = product.Rating,
                CategoryId = product.CategoryId,
                Price = product.Price
            };
            await _SendCategoriesWithViewBag();
            return View(product);
        }
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            if(!ModelState.IsValid)
            {
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "Bele bir category movcud deyil!");
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            if (!vm.Image?.CheckType("image") ?? false)
            {
                ModelState.AddModelError("image", "Zehmet olmasa image tipinde data daxil edin!");
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            if (!vm.Image?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("image", "Zehmet olmasa max 2mb-liq image tipinde data daxil edin!");
                await _SendCategoriesWithViewBag();
                return View(vm);
            }
            var isexistProduct = await _context.Products.FindAsync(vm.Id);
            if (isexistProduct == null)
                return BadRequest();
            isexistProduct.Name = vm.Name;
            isexistProduct.Description = vm.Description;
            isexistProduct.Rating = vm.Rating;
            isexistProduct.CategoryId = vm.CategoryId;
            isexistProduct.Price = vm.Price;
            if(vm.Image is { })
            {
                string imagePath = await vm.Image.FileUploadAsync(_folderPath);
                string oldImagePath = Path.Combine(_folderPath, isexistProduct.ImagePath);
                ExtensionMethods.FileDelete(oldImagePath);
                isexistProduct.ImagePath = imagePath;
            }
            _context.Products.Update(isexistProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task _SendCategoriesWithViewBag()
        {
            var categories = await _context.Categories.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
            ViewBag.Categories = categories;
        }
    }
}
