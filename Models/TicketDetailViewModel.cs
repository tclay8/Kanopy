using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kanopy.Models
{
    public class TicketDetailViewModel
    {
        public Ticket Ticket { get; set; }
        public SelectList AssignUser { get; set; }

    }
}