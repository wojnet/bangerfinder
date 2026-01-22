using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Backend.bangerback.Infrastructure.Data;



namespace FinalAppTemplate.Models.Attributes
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var db = context.HttpContext.RequestServices.GetService<AppDbContext>();

            // CHANGE: Read from Header "Authorization", not Cookie
            string authHeader = context.HttpContext.Request.Headers["Authorization"];

            // Expect format: "Bearer <token>"
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                // CHANGE: Return 401 Unauthorized, NEVER Redirect to Login view
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var session = db.Sessions.Include(s => s.User).FirstOrDefault(s => s.Token == token);

            if (session == null || session.ExpiresAt < DateTime.Now)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            context.HttpContext.Items["CurrentUser"] = session.User;
            base.OnActionExecuting(context);
        }

    }
}
