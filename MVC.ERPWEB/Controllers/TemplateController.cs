using Microsoft.AspNetCore.Mvc;

namespace GRP.MVCWEB.Controllers
{
    public class TemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }
        
        public IActionResult Team()
        {
            return View();
        }
        
        public IActionResult Calendar()
        {
            return View();
        }
        
        public IActionResult EmailCompose()
        {
            return View();
        }
        
        public IActionResult EmailInbox()
        {
            return View();
        }
        
        public IActionResult EmailDetails()
        {
            return View();
        }
        
        public IActionResult DataTable()
        {
            return View();
        }

    }
}
