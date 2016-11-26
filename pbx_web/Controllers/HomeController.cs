using System.Collections.Generic;
using System.Web.Mvc;

namespace pbx_web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult PhoneLines()
        {
            return View(new List<string>() { "asdf", "bdfs", "cdfsdaf" });
        }

    }
}
