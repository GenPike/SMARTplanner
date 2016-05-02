using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SMARTplanner.Logic.Contracts;
using Microsoft.AspNet.Identity;
using SMARTplanner.Entities.Helpers;
using SMARTplanner.Models;
using AutoMapper;

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
                var collectionViewModel = result.TargetCollection
                    .Select(project => Mapper.Map<ProjectListItemViewModel>(project))
                    .ToList();

                return View(collectionViewModel);
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("CustomError");
            }
        }
    }
}