using Kanopy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kanopy.Helpers;

namespace Kanopy.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [RequireHttps]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserDashboardViewModel model = new UserDashboardViewModel();
            ProjectsHelper projectsHelper = new ProjectsHelper();

            var allProj = db.Projects.ToList();
            var userId = User.Identity.GetUserId();

            foreach (var proj in allProj)
            {
                //if (projectsHelper.IsUserOnProject(userId, proj.Id))
                //{
                    CompletedViewModel thisModel = new CompletedViewModel();
                    var compTicket = db.Tickets.Where(t => t.ProjectId == proj.Id && t.TicketStatus.Name == "Completed").ToList().Count();
                    var totTicket = db.Tickets.Where(t => t.ProjectId == proj.Id).ToList().Count();
                    thisModel.Project = proj;
                    thisModel.CompletedTickets = compTicket;
                    thisModel.TotalTickets = totTicket;
                    thisModel.PercentComplete = totTicket == 0 ? 0 : (int)(((double)compTicket / totTicket) * 100);
                    model.CompTickets.Add(thisModel);
                //}
            }

            model.Urgent = db.Tickets.Where(t => t.TicketPriority.Name.Contains("Urgent")).Count();
            model.High = db.Tickets.Where(t => t.TicketPriority.Name.Contains("High")).Count();
            model.Medium = db.Tickets.Where(t => t.TicketPriority.Name.Contains("Medium")).Count();
            model.Low = db.Tickets.Where(t => t.TicketPriority.Name.Contains("Low")).Count();

            model.New = db.Tickets.Where(t => t.TicketStatus.Name.Contains("New")).Count();
            model.OnHold = db.Tickets.Where(t => t.TicketStatus.Name.Contains("On Hold")).Count();
            model.InProgress = db.Tickets.Where(t => t.TicketStatus.Name.Contains("In Progress")).Count();
            model.Completed = db.Tickets.Where(t => t.TicketStatus.Name.Contains("Completed")).Count();

            model.ProductionFix = db.Tickets.Where(t => t.TicketType.Name.Contains("Production Fix")).Count();
            model.ProductionTask = db.Tickets.Where(t => t.TicketType.Name.Contains("Production Task")).Count();
            model.SoftwareUpdate = db.Tickets.Where(t => t.TicketType.Name.Contains("Software Update")).Count();

            if (User.IsInRole("Admin"))
            {
                model.Urgent = db.Tickets.Where(t => t.TicketPriority.Name.Contains("Urgent")).Count();
                model.New = db.Tickets.Where(t => t.TicketStatus.Name.Contains("New")).Count();
                model.OnHold = db.Tickets.Where(t => t.TicketStatus.Name.Contains("On Hold")).Count();
                model.NewUsers = db.Users.Where(u => u.Roles.Count() == 0).ToList();
                model.Projects = db.Projects.ToList();


            }
            else if (User.IsInRole("ProjectManager"))
            {

                var projects = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();

                var tickets = projects.SelectMany(t => t.Tickets).ToList();

                model.Urgent = tickets.Where(t => t.TicketPriority.Name.Contains("Urgent")).Count();
                model.New = db.TicketStatuses.Where(t => t.Name.Contains("New")).Count();
                model.OnHold = db.TicketStatuses.Where(t => t.Name.Contains("On Hold")).Count();
                model.InProgress = db.TicketStatuses.Where(t => t.Name.Contains("In Progress")).Count();
                model.Projects = projects;


            }
            else if (User.IsInRole("Developer"))
            {

                var user = db.Users.Find(userId);

                var projects = user.Projects.ToList();

                var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();

                model.Urgent = tickets.Where(t => t.TicketPriority.Name.Contains("Urgent")).Count();
                model.New = tickets.Where(t => t.TicketStatus.Name.Contains("New")).Count();
                model.InProgress = tickets.Where(t => t.TicketStatus.Name.Contains("InProgress")).Count();
                model.AllTickets = tickets.Count();
                model.Projects = projects;


            }
            else if (User.IsInRole("Submitter"))
            {

                var user = db.Users.Find(userId);

                var projects = user.Projects.ToList();

                var tickets = db.Tickets.Where(t => t.OwnerUserId == userId);

                model.Completed = tickets.Where(t => t.TicketPriority.Name.Contains("Completed")).Count();
                model.AllTickets = tickets.Count();
                model.Projects = projects;


            }


            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Models.EmailModel model = new Models.EmailModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>(0)</bold>" +
                        "({1})</p><p>Message:</p><p>{2}</p>";
                    model.Body = "This is a message from your blog site.  The name and" + "the email of the contacting person is above.";

                    var svc = new EmailService();
                    var msg = new IdentityMessage()
                    {
                        Subject = "Contact From Portfolio Site",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        Destination = "jtara95@gmail.com"
                    };

                    await svc.SendAsync(msg);
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                    return View(model);
                }
            }
            else { return View(model); }
        }

        private ApplicationDbContext db = new ApplicationDbContext();
        [ChildActionOnly]
        public PartialViewResult _SideNav()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                return PartialView(user);
            }
            return PartialView();
        }
        [ChildActionOnly]
        public PartialViewResult _RightTopNav()
        {
            var notifications = new List<TicketNotification>();
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                notifications = db.TicketNotifications.Where(t => t.ReceipientId == userId).ToList();
                return PartialView(notifications);
            }
            return PartialView(notifications);
        }

        public ActionResult AssignedTicket()
        {
            var userId = User.Identity.GetUserId();
            var model = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();

            return View(model);
        }


        public ActionResult CompletedTicket()
        {
            var model = db.Tickets.Where(t => t.TicketStatus.Name.Contains("Completed")).ToList();

            return View(model);
        }

        public ActionResult InProgressTicket()
        {
            var model = db.Tickets.Where(t => t.TicketStatus.Name.Contains("InProgress")).ToList();

            return View(model);
        }

        public ActionResult OnHoldTicket()
        {
            var model = db.Tickets.Where(t => t.TicketStatus.Name.Contains("OnHold")).ToList();

            return View(model);
        }

        public ActionResult UrgentTicket()
        {
            var model = db.Tickets.Where(t => t.TicketPriority.Name.Contains("Urgent")).ToList();

            return View(model);
        }

        public ActionResult NewTicket()
        {
            var model = db.Tickets.Where(t => t.TicketStatus.Name.Contains("New")).ToList();

            return View(model);
        }



    }
}