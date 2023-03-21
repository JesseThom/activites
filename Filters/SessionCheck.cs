//must be in Filters Folder
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? UserId = context.HttpContext.Session.GetInt32("uuid");
        if (UserId == null)
        {
            context.Result = new RedirectToActionResult("Index","Home", null);
        }
    }
}