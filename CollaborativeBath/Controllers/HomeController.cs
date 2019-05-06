using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    /// <summary>
    /// Controller for the landing page area.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Return the landing page to no authenticated users. Else
        /// redirects them to /Folder/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Folder");
            }
        }
    }
}