using Kanopy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.ApplicationServices;

namespace Kanopy.Helpers
{
    public class HistoriesHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private TicketHistory history = new TicketHistory();
        public void GenerateHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            var user = db.Users.Find(userId);
            if(oldTicket.Title != newTicket.Title)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Title";
                history.OldValue = oldTicket.Title;
                history.NewValue = newTicket.Title;
                history.Changed = DateTime.Now;
                history.ChangedBy = user.FirstName;

                db.TicketHistories.Add(history);
                db.SaveChanges();

            }
            if (oldTicket.Description != newTicket.Description)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Description";
                history.OldValue = oldTicket.Description;
                history.NewValue = newTicket.Description;
                history.Changed = DateTime.Now;
                history.ChangedBy = user.FirstName;

                db.TicketHistories.Add(history);
                db.SaveChanges();

            }
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                history.TicketId = newTicket.Id;
                history.Property = "AssignedToUserId";
                history.OldValue = oldTicket.AssignedToUserId;
                history.NewValue = newTicket.AssignedToUserId;
                history.Changed = DateTime.Now;
                history.ChangedBy = user.FirstName;

                db.TicketHistories.Add(history);
                db.SaveChanges();

            }
            if (oldTicket.TicketPriority.Name != newTicket.TicketPriority.Name)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Priority";
                history.OldValue = oldTicket.TicketPriority.Name;
                history.NewValue = newTicket.TicketPriority.Name;
                history.Changed = DateTime.Now;
                history.ChangedBy = user.FirstName;

                db.TicketHistories.Add(history);
                db.SaveChanges();

            }
            if (oldTicket.TicketStatus.Name != newTicket.TicketStatus.Name)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Status";
                history.OldValue = oldTicket.TicketStatus.Name;
                history.NewValue = newTicket.TicketStatus.Name;
                history.Changed = DateTime.Now;
                history.ChangedBy = user.FirstName;

                db.TicketHistories.Add(history);
                db.SaveChanges();

            }
            if (oldTicket.TicketType.Name != newTicket.TicketType.Name)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Type";
                history.OldValue = oldTicket.TicketType.Name;
                history.NewValue = newTicket.TicketType.Name;
                history.Changed = DateTime.Now;
                history.ChangedBy = user.FirstName;

                db.TicketHistories.Add(history);
                db.SaveChanges();

            }





        }
    }
}