using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BhuttaBoutiqueERP.Services.Production;
using BhuttaBoutiqueERP.Services.Inventory;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Controllers.Production
{
    [Route("Production/StockCheck")]
    public class StockCheckController : Controller
    {
        private readonly IStockCheckService _stockCheckService;
        private readonly IItemService _itemService;

        public StockCheckController(IStockCheckService stockCheckService, IItemService itemService)
        {
            _stockCheckService = stockCheckService;
            _itemService = itemService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var items = await _itemService.GetAllAsync();

            // ✅ N+1 Query Fix: Ek hi baar mein saare Items ke Available Stock nikaalo
            var allStockData = await _stockCheckService.GetAllAvailableStockAsync();

            var stockList = new List<StockCheckViewModel>();

            foreach (var item in items)
            {
                var available = allStockData.ContainsKey(item.Id) ? allStockData[item.Id] : item.Stock;
                stockList.Add(new StockCheckViewModel
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Category = item.Category,
                    Stock = item.Stock,
                    ReorderLevel = item.ReorderLevel,
                    AvailableStock = available
                });
            }

            return View("~/Views/Production/StockCheck/Index.cshtml", stockList);
        }

        [HttpGet("Check/{itemId:int}/{quantity:decimal}")]
        public async Task<IActionResult> CheckAvailability(int itemId, decimal quantity)
        {
            var isAvailable = await _stockCheckService.CheckAvailabilityAsync(itemId, quantity);
            return Json(new { available = isAvailable });
        }

        [HttpGet("Available/{itemId:int}")]
        public async Task<IActionResult> GetAvailableStock(int itemId)
        {
            var available = await _stockCheckService.GetAvailableStockAsync(itemId);
            return Json(new { availableStock = available });
        }
    }
}