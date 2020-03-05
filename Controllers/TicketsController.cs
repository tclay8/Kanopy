using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kanopy.Helpers;
using Kanopy.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;

namespace Kanopy.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HistoriesHelper HistoryHelper = new HistoriesHelper();
        private ProjectsHelper projectsHelper = new ProjectsHelper();
        private UserRolesHelper UserRolesHelper = new UserRolesHelper();
        // GET: Tickets
        public ActionResult Index()
        {
            TicketIndexViewModel model = new TicketIndexViewModel();
            var tickets = db.Tickets;
            var myTickets = new List<Ticket>();
            var devTickets = new List<Ticket>();

            if (User.IsInRole("Admin"))
            {
                myTickets = tickets.ToList();

            }
            else if (User.IsInRole("ProjectManager"))
            {
                var userId = User.Identity.GetUserId();

                var projects = db.Projects.Where(t => t.ProjectManagerId == userId).ToList();

                myTickets = projects.SelectMany(t => t.Tickets).ToList();

                
            }
            else if (User.IsInRole("Developer"))
            {
                var userId = User.Identity.GetUserId();

                var user = db.Users.Find(userId);

                var projects = user.Projects.ToList();

                myTickets = projects.SelectMany(t => t.Tickets).ToList();

                devTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();

            }
            else if (User.IsInRole("Submitter"))
            {
                var userId = User.Identity.GetUserId();

                var user = db.Users.Find(userId);

                myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();

            }
            model.MyTickets = myTickets;
            model.AssignedTickets = devTickets;

            return View(model);

        }


        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            TicketDetailViewModel model = new TicketDetailViewModel();
            model.Ticket = ticket;
            var users = UserRolesHelper.UsersInRole("Developer").ToList();
            model.AssignUser = new SelectList(UserRolesHelper.UsersInRole("Developer"), "Id", "FirstName");
            ViewBag.AssignedToUserId = new SelectList(projectsHelper.ListUsersProjectsInRole(ticket.ProjectId, "Developer"), "Id", "FullName", ticket.AssignedToUserId);

            return View(model);
        }

        // GET: Tickets/Create
        [Authorize (Roles ="Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(projectsHelper.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize (Roles = "Submitter")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.AssignedToUserId = null;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            var userId = User.Identity.GetUserId();
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(projectsHelper.ListUserProjects(userId), "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Developer, ProjectManager, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(projectsHelper.ListUserProjects(userId), "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Developer, ProjectManager, Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {

            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                ticket.Updated = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                NotificationManager.ManageTicketNotifications(oldTicket, newTicket);

                //var userId = User.Identity.GetUserId();
                HistoryHelper.GenerateHistory(oldTicket, newTicket, userId);
                return RedirectToAction("Index");
            }


            UserRolesHelper helper = new UserRolesHelper();

            var users = helper.UsersInRole("Developer").ToList();

            


            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(projectsHelper.ListUserProjects(userId), "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ProjectManager, Admin")]
        public ActionResult AssignTicket(int? id)
        {
            UserRolesHelper helper = new UserRolesHelper();

            var ticket = db.Tickets.Find(id);
            var users = helper.UsersInRole("Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FirstName", ticket.AssignedToUserId);

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager, Admin")]

        public async Task<ActionResult> AssignTicket(int ticketId, string assignedId)
        {

            var ticket = db.Tickets.Find(ticketId);
            ticket.AssignedToUserId = assignedId;

            db.SaveChanges();

            var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

            try
            {
                EmailService ems = new EmailService();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = db.Users.Find(assignedId);

                msg.Body = "You have been assigned a new Ticket." + Environment.NewLine + "Please click the following link to view the details " + "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";

                msg.Destination = user.Email;
                msg.Subject = "Invite to Household";

                await ems.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }
            return RedirectToAction("Index");
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
