using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BhuttaBoutiqueERP.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            // Agar AJAX request hai toh sirf clean content return ho, warna poora View
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Home/Index.cshtml");
            }

            return View("~/Views/Home/Index.cshtml");
        }
    }
}