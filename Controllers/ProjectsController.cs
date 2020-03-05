using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kanopy.Models;
using Kanopy.Helpers;
using Microsoft.AspNet.Identity;

namespace Kanopy.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        public ActionResult Index()
        {
            List<Project> model = new List<Project>();

            if (User.IsInRole("Admin"))
            {
                model = db.Projects.Where(p => p.IsDeleted == false).ToList();

            }
            else if (User.IsInRole("ProjectManager")) 
            {
                var userId = User.Identity.GetUserId();

                model = db.Projects.Where(p => p.ProjectManagerId == userId && p.IsDeleted == false).ToList();

            }
            else if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                var userId = User.Identity.GetUserId();

                var user = db.Users.Find(userId);

                model = user.Projects.ToList();

            }
            ProjectsIndexViewModel vm = new ProjectsIndexViewModel();
            vm.Projects = model;
            return View(vm);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            project.New = db.Tickets.Where(t => t.TicketStatus.Name.Contains("New") && t.ProjectId == id).Count();
            project.OnHold = db.Tickets.Where(t => t.TicketStatus.Name.Contains("On Hold") && t.ProjectId == id).Count();
            project.InProgress = db.Tickets.Where(t => t.TicketStatus.Name.Contains("In Progress") && t.ProjectId == id).Count();
            project.Completed = db.Tickets.Where(t => t.TicketStatus.Name.Contains("Completed") && t.ProjectId == id).Count();


            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Details")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Details")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        private ProjectsHelper helper = new ProjectsHelper();

        //GET: Roles
        [Authorize]
        public ActionResult AssignProject()
        {
            AssignProjectViewModel model = new AssignProjectViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.ProjectId = new SelectList(db.Projects, "Id", "Name");
            model.UserRoles = db.Users.ToList();


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AssignProject(string UserId, int ProjectId)
        {
            if (helper.AddUserToProject(UserId, ProjectId))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveProject()
        {
            RemoveProjectViewModel model = new RemoveProjectViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.ProjectId = new SelectList(db.Projects, "Id", "Name");
            model.UserRoles = db.Users.ToList();


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult RemoveProject(string userId, int projectId)
        {
            if (helper.RemoveUserFromProject(userId, projectId))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }


        //GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            project.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[HandleError]
        //public ActionResult SaveForm1(ProjectsIndexViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SaveChanges();


        //        //Return Success message
        //        ViewBag.Message = "Edit saved";
        //        ModelState.Clear();
        //        return View("Index");
        //    }
        //    return View("Index", model);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
