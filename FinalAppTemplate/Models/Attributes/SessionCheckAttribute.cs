using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FinalAppTemplate.Models; // Update with your namespace
using Microsoft.EntityFrameworkCore;
using FinalAppTemplate.Data;



namespace FinalAppTemplate.Models.Attributes
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. Get the Database Context (Accessing services manually in a filter)
            var db = context.HttpContext.RequestServices.GetService<FinalAppTemplateDbContext>(); // REPLACE 'YourDbContext' with your actual DB context class name

            // 2. Get Token
            var sessionToken = context.HttpContext.Request.Cookies["UserSession"];

            if (string.IsNullOrEmpty(sessionToken))
            {
                context.Result = new RedirectToActionResult("Login", "UserCreation", null);
                return;
            }

            // 3. Verify in DB
            // Note: Filters run synchronously by default. 
            // For simple apps, .Result is okay, or you can make an Async ActionFilter.
            var session = db.Sessions.Include(s => s.User).FirstOrDefault(s => s.Token == sessionToken);

            if (session == null || session.ExpiresAt < DateTime.Now)
            {
                context.Result = new RedirectToActionResult("Login", "UserCreation", null);
                return;
            }

            // 4. (Optional) Store the user object so the Controller can use it easily
            context.HttpContext.Items["CurrentUser"] = session.User;

            base.OnActionExecuting(context);
        }

    }
}
