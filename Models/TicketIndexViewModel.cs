using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanopy.Models
{
    public class TicketIndexViewModel
    {
        public ICollection<Ticket> MyTickets { get; set; }
        public ICollection<Ticket> AssignedTickets { get; set; }
    }
}