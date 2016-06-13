using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using SFA.DAS.EmployerUsers.WebClientComponents;
using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

namespace SFA.DAS.EmployerPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AuthoriseActiveUser2]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class AuthoriseActiveUser2Attribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                return;
            }

            var user = filterContext.HttpContext.User as ClaimsPrincipal;
            var requiresVerification = user?.Claims.FirstOrDefault(c => c.Type == DasClaimTypes.RequiresVerification)?.Value;
            if (string.IsNullOrEmpty(requiresVerification) || requiresVerification.Equals("true", System.StringComparison.OrdinalIgnoreCase))
            {
                var configuration = ConfigurationFactory.Current.Get();
                filterContext.Result = new RedirectResult(configuration.AccountActivationUrl);
            }
        }
    }
}