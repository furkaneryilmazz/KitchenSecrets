using KitchenSecrets.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KitchenSecrets.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]   
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
            if(user != null)
            {
                //Oturum veya kimlik doğrulama işlemi
                HttpContext.Session.SetString("UserId",user.UserId.ToString());
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                return RedirectToAction("Index","Default");
            }
            ViewBag.ErrorMessage = "Geçersiz giriş denemesi.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Default");
        }

        public IActionResult Index()
        {
            var articles = _context.Products.Include(a=>a.User).ToList();
            return View(articles);
        }

        //public async Task SignInUserAsync(int userId, ClaimsPrincipal userPrincipal)
        //{
        //    var identity = new ClaimsIdentity(userPrincipal.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(identity));
        //}

    }
}
