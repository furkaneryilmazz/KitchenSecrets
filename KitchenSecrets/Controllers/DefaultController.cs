using Microsoft.AspNetCore.Mvc;

namespace KitchenSecrets.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
