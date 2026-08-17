using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BhuttaBoutiqueERP.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            string errorMessage = "Aik unexpected system error aa gaya hai. Baraye meharbani dobara koshish karein.";

            // 1. Safe check for database duplicate/unique constraint errors
            if (exception is DbUpdateException || (exception.InnerException != null && exception.InnerException.Message.Contains("UNIQUE")))
            {
                errorMessage = "Yeh record ya CNIC/Phone pehle se system mein mojood hai. Duplicate entry ki ijazat nahi hai.";
            }

            // Check if the request is coming via AJAX (Modal)
            bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                // Return JSON for modals so the app never crashes
                context.Result = new JsonResult(new
                {
                    success = false,
                    message = errorMessage
                });
                context.ExceptionHandled = true;
            }
            else
            {
                // Redirect for normal page requests
                context.Result = new RedirectToActionResult("Error", "Home", new { message = errorMessage });
                context.ExceptionHandled = true;
            }
        }
    }
}