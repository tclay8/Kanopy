using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanopy.Models
{
    public class CompletedViewModel
    {
        public Project Project { get; set; }
        public int CompletedTickets { get; set; }
        public int TotalTickets { get; set; }
        public int PercentComplete { get; set; }
    }
}