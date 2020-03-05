using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanopy.Models
{
    public class UserDashboardViewModel
    {
        public ChangeNameViewModel ChangeName { get; set; }
        public TicketIndexViewModel TicketIndex { get; set; }


        public int Urgent { get; set; }
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }
        public int New { get; set; }
        public int OnHold { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int ProductionFix { get; set; }
        public int ProductionTask { get; set; }
        public int SoftwareUpdate { get; set; }

        public int AllTickets { get; set; }

        public ICollection<CompletedViewModel> CompTickets { get; set; }

        public ICollection<ApplicationUser> NewUsers { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Project> Projects { get; set; }

        public UserDashboardViewModel()
        {
            ChangeName = new ChangeNameViewModel();
            TicketIndex = new TicketIndexViewModel();
            CompTickets = new List<CompletedViewModel>();
        }

        public class ChartData
        {
            public List<string> labels { get; set; }
            public DataSetData datasets { get; set; }
        }
        public class DataSetData
        {
            public List<int> data { get; set; }
            public List<string> backgroundColor { get; set; }
            public List<string> hoverBackgroundColor { get; set; }

        }
    }
}