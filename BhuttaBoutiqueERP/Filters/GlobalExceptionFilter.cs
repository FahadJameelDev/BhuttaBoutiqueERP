using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace BhuttaBoutiqueERP.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            string errorMessage = "Aik unexpected system error aa gaya hai. Baraye meharbani dobara koshish karein.";

            if (exception is DbUpdateException dbEx)
            {
                // Inner exception ka poora text nikaalo (SqlException hoti hai andar)
                string innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;

                // ✅ 1. FOREIGN KEY Violation (Record kisi aur jagah use ho raha hai)
                if (innerMessage.Contains("REFERENCE constraint") ||
                    innerMessage.Contains("FOREIGN KEY") ||
                    (dbEx.InnerException is SqlException sqlEx1 && sqlEx1.Number == 547))
                {
                    errorMessage = "Ye record delete nahi ho sakta kyunki ye kisi aur jagah (jaise Stock, Order, ya Payroll) mein use ho raha hai. Pehle wo related records hataayein.";
                }
                // ✅ 2. UNIQUE Violation (Duplicate entry)
                else if (innerMessage.Contains("UNIQUE") ||
                         innerMessage.Contains("duplicate key") ||
                         (dbEx.InnerException is SqlException sqlEx2 && (sqlEx2.Number == 2601 || sqlEx2.Number == 2627)))
                {
                    errorMessage = "Yeh record ya CNIC/Phone pehle se system mein mojood hai. Duplicate entry ki ijazat nahi hai.";
                }
            }

            bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                context.Result = new JsonResult(new
                {
                    success = false,
                    message = errorMessage
                });
                context.HttpContext.Response.StatusCode = 200; // ✅ Zaroori: Taake AJAX success block mein message mil jaye
            }
            else
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { message = errorMessage });
            }

            context.ExceptionHandled = true;
        }
    }
}