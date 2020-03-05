using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kanopy.Helpers;
using System.Net;
using Kanopy.Models;

namespace Kanopy.Controllers
{
    [RequireHttps]
    public class RolesController : Controller
    {
        private UserRolesHelper helper = new UserRolesHelper();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles
        [Authorize(Roles = "Admin")]
        public ActionResult AssignUserRole()
        {
            AssignUserRoleViewModel model = new AssignUserRoleViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.RoleName = new SelectList(db.Roles, "Name", "Name");
            model.UserRoles = new List<UserRoleVM>();

            foreach(var user in db.Users.ToList())
            {
                model.UserRoles.Add(new UserRoleVM
                {
                    UserName = $"{user.FirstName} {user.LastName}",
                    RoleName = helper.ListUserRoles(user.Id).FirstOrDefault()
                }
                    );
            }


            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AssignUserRole(string UserId, string RoleName)
        {
            if (helper.AddUserToRole(UserId, RoleName))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveUserRole()
        {
            RemoveUserRoleViewModel model = new RemoveUserRoleViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.RoleName = new SelectList(db.Roles, "Name", "Name");
            model.UserRoles = new List<UserRoleVM>();

            foreach (var user in db.Users.ToList())
            {
                model.UserRoles.Add(new UserRoleVM
                {
                    UserName = $"{user.FirstName} {user.LastName}",
                    RoleName = helper.ListUserRoles(user.Id).FirstOrDefault()
                }
                    );
            }



            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult RemoveUserRole(string UserId, string RoleName)
        {
            if (helper.RemoveUserFromRole(UserId, RoleName))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
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