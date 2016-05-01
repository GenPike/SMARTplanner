using System;
using System.Web;
using System.Web.Mvc;

namespace SMARTplanner.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }
    }
}