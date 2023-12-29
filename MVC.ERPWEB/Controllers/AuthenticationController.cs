using Microsoft.AspNetCore.Mvc;

namespace MVC.ERPWEB.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }
    }
}
