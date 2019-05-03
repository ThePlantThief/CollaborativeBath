using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    public class HomeController : Controller
    {
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}