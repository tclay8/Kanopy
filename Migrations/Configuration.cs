namespace Kanopy.Migrations
{
    using System;
    using Kanopy.Models;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Launch();

            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole();

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                role = new IdentityRole { Name = "Developer" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                role = new IdentityRole { Name = "Submitter" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                role = new IdentityRole { Name = "ProjectManager" };
                manager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];

            if (!context.Users.Any(u => u.Email == "tclay@coderfoundry.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "tclay@coderfoundry.com",
                    Email = "tclay@coderfoundry.com",
                    FirstName = "Tara",
                    LastName = "Clay",
                    AvatarPath = "",
                    //DisplayName = "TClay"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Admin",
                        "ProjectManager",
                        "Developer",
                        "Submitter"
                    });
            }

            if (!context.Users.Any(u => u.Email == "admin@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@demo.com",
                    Email = "admin@demo.com",
                    FirstName = "Pam",
                    LastName = "Beasley",
                    AvatarPath = "~/Template/img/Pam.jpg",
                    //DisplayName = "ADMIN"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Admin"
                    });
            }

            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoAdmin@mailinator.com",
                    Email = "DemoAdmin@mailinator.com",
                    FirstName = "Angela",
                    LastName = "Martin",
                    AvatarPath = "~/Template/img/Angela.jpg",
                    //DisplayName = "ADMIN"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Admin"
                    });
            }

            if (!context.Users.Any(u => u.Email == "DemoProjectManager@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoProjectManager@mailinator.com",
                    Email = "DemoProjectManager@mailinator.com",
                    FirstName = "Oscar",
                    LastName = "Martinez",
                    AvatarPath = "~/Template/img/Oscar.jpg",
                    //DisplayName = "ADMIN"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "ProjectManager"
                    });
            }

            if (!context.Users.Any(u => u.Email == "DemoDeveloper@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoDeveloper@mailinator.com",
                    Email = "DemoDeveloper@mailinator.com",
                    FirstName = "Andy",
                    LastName = "Bernard",
                    AvatarPath = "",
                    //DisplayName = "ADMIN"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Developer"
                    });
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitter@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "DemoSubmitter@mailinator.com",
                    Email = "DemoSubmitter@mailinator.com",
                    FirstName = "Kelly",
                    LastName = "Kapoor",
                    AvatarPath = "~/Template/img/Kelly.jpg",
                    //DisplayName = "ADMIN"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Submitter"
                    });
            }


            if (!context.Users.Any(u => u.Email == "manager@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "manager@demo.com",
                    Email = "manager@demo.com",
                    FirstName = "Dwight",
                    LastName = "Shrute",
                    AvatarPath = "~/Template/img/Dwight.jpg",
                    //DisplayName = "MANGR"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "ProjectManager"
                    });
            }

            if (!context.Users.Any(u => u.Email == "developer@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "developer@demo.com",
                    Email = "developer@demo.com",
                    FirstName = "Jim",
                    LastName = "Halpert",
                    AvatarPath = "~/Template/img/Jim.jpg",
                    //DisplayName = "DEVPR"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Developer"
                    });
            }

            if (!context.Users.Any(u => u.Email == "submitter@demo.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "submitter@demo.com",
                    Email = "submitter@demo.com",
                    FirstName = "Kevin",
                    LastName = "Malone",
                    AvatarPath = "~/Template/img/Kevin.jpg",
                    //DisplayName = "SUBMT"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Submitter"
                    });
            }
            if (!context.Users.Any(u => u.Email == "coderfoundry@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "coderfoundry@mailinator.com",
                    Email = "coderfoundry@mailinator.com",
                    FirstName = "David",
                    LastName = "Wallace",
                    AvatarPath = "",
                    //DisplayName = "SUBMT"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Submitter"
                    });
            }


            if (!context.TicketPriorities.Any(u => u.Name == "High"))
            { context.TicketPriorities.Add(new Models.TicketPriority { Name = "High" }); }

            if (!context.TicketPriorities.Any(u => u.Name == "Medium"))
            { context.TicketPriorities.Add(new Models.TicketPriority { Name = "Medium" }); }

            if (!context.TicketPriorities.Any(u => u.Name == "Low"))
            { context.TicketPriorities.Add(new Models.TicketPriority { Name = "Low" }); }

            if (!context.TicketPriorities.Any(u => u.Name == "Urgent"))
            { context.TicketPriorities.Add(new Models.TicketPriority { Name = "Urgent" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Production Fix"))
            { context.TicketTypes.Add(new Models.TicketType { Name = "Production Fix" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Production Task"))
            { context.TicketTypes.Add(new Models.TicketType { Name = "Production Task" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Software Update"))
            { context.TicketTypes.Add(new Models.TicketType { Name = "Software Update" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "New"))
            { context.TicketStatuses.Add(new Models.TicketStatus { Name = "New" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "In Progress"))
            { context.TicketStatuses.Add(new Models.TicketStatus { Name = "In Progress" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "Completed"))
            { context.TicketStatuses.Add(new Models.TicketStatus { Name = "Completed" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "On Hold"))
            { context.TicketStatuses.Add(new Models.TicketStatus { Name = "On Hold" }); }
            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
