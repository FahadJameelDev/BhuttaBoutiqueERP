using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BhuttaBoutiqueERP.Models.ViewModels;

namespace BhuttaBoutiqueERP.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        // GET: /Home/Index
        [HttpGet("Home/Index")]
        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalOrders = 120,
                PendingOrders = 15,
                TodayRevenue = 45000,
                TotalEmployees = 8,
                MonthlyRevenue = 1250000,
                WeeklySales = new List<WeeklySale>
                {
                    new WeeklySale { Day = "Mon", Total = 15000 },
                    new WeeklySale { Day = "Tue", Total = 22000 },
                    new WeeklySale { Day = "Wed", Total = 18000 },
                    new WeeklySale { Day = "Thu", Total = 30000 },
                    new WeeklySale { Day = "Fri", Total = 25000 },
                    new WeeklySale { Day = "Sat", Total = 40000 },
                    new WeeklySale { Day = "Sun", Total = 35000 }
                },
                RecentOrders = new List<RecentOrder>()
            };

            // Agar AJAX request hai toh sirf partial view return ho taake main content area update ho
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Home/Index.cshtml", model);
            }

            return View("~/Views/Home/Index.cshtml", model);
        }
    }
}