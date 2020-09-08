using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParksProject.Models;
using NationalParksProject.Services.IRepository;
using System.Threading.Tasks;

namespace NationalParksProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (user == null) return View(new User());

            var loggedInUser = await _userRepository.LoginAsync(AppConstants.LoginPath, user);
            if (loggedInUser == null)
            {
                return View(user);
            }

            HttpContext.Session.SetString("token", loggedInUser.Token);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // public async Task<IActionResult> Register(User user)
        // {
        //
        // }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");

            return RedirectToAction("Index", "Home");
        }
    }
}
