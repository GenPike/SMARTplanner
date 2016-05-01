using System;
using System.Web;
using System.Web.Mvc;
using SMARTplanner.Logic.Contracts;
using Microsoft.AspNet.Identity;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService ps)
        {
            _projectService = ps;
        }

        // GET: Project
        public ActionResult Index(ProjectFilter filter = ProjectFilter.All, int page = 1)
        {
            var result = _projectService.GetProjectsPaged(User.Identity.GetUserId(),
                filter, page);

            if (!result.ErrorHandled)
            {

                return View();
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("CustomError");
            }
        }
    }
}