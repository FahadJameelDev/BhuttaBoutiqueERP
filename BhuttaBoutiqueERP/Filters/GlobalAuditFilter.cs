using Microsoft.AspNetCore.Mvc.Filters;
using BhuttaBoutiqueERP.Services.Reports;

namespace BhuttaBoutiqueERP.Filters
{
    public class GlobalAuditFilter : IAsyncActionFilter
    {
        private readonly IActivityLogService _activityLogService;

        public GlobalAuditFilter(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next(); // Action execute hone do

            // Sirf tab log karo jab action successful ho aur request GET na ho
            if (resultContext.Exception == null && context.HttpContext.Request.Method != "GET")
            {
                var controllerName = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
                var actionName = context.RouteData.Values["action"]?.ToString() ?? "Unknown";

                string actionType = "Action";
                string lowerAction = actionName.ToLower();

                if (lowerAction.Contains("create") || lowerAction.Contains("add") || lowerAction.Contains("insert"))
                    actionType = "Create";
                else if (lowerAction.Contains("edit") || lowerAction.Contains("update"))
                    actionType = "Update";
                else if (lowerAction.Contains("delete") || lowerAction.Contains("remove"))
                    actionType = "Delete";

                if (actionType == "Create" || actionType == "Update" || actionType == "Delete")
                {
                    var userName = context.HttpContext.User.Identity?.Name ?? "admin@bhuttaboutique.com";
                    var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                    var description = $"Executed {actionName} in {controllerName} module";

                    await _activityLogService.LogActivityAsync(userName, actionType, controllerName, description, ipAddress);
                }
            }
        }
    }
}