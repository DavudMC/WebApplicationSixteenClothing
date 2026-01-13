using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplicationSixteenClothing.Models;
using WebApplicationSixteenClothing.ViewModels.UserViewModels;

namespace WebApplicationSixteenClothing.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signinManager,RoleManager<IdentityRole> _roleManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) 
            {
                return View(vm);
            }
            AppUser user = new()
            {
                UserName = vm.UserName,
                Email = vm.Email,
                FullName = vm.FullName,
            };
            var result = await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user,"Member");
            
            await _signinManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(vm);
            }
            var result = await _signinManager.PasswordSignInAsync(user, vm.Password, false, false);
            if (!result.Succeeded) 
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Admin"
            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Member"
            });
            return Ok("Roles created succesfully");
        }
    }
}
