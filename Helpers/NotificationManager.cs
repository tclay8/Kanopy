using Kanopy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanopy.Helpers
{
    public static class NotificationManager
    {

        public static void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            ManageGeneralAssignmentNotification(oldTicket, newTicket);
            ManagePropertyChangeNotifications(oldTicket, newTicket);

        }

        private static void ManageGeneralAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var assigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
            var unassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
            var reassigned = newTicket.AssignedToUserId != null && newTicket.AssignedToUserId != oldTicket.AssignedToUserId;

            var newNotification = new TicketNotification();
            newNotification.TicketId = newTicket.Id;


            if (assigned)
            {

                newNotification.ReceipientId = newTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been assigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
                
            }
            else if (unassigned)
            {
                newNotification.ReceipientId = oldTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been unassigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
            else if (reassigned)
            {
                newNotification.ReceipientId = newTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been assigned to Ticket {newTicket.Title}  {newTicket.Id}";
                GenerateNotification(newNotification);

                newNotification.ReceipientId = oldTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been unassigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
            else
            {

            }

        }

        private static void ManagePropertyChangeNotifications(Ticket oldTicket, Ticket newTicket)
        {
            if (newTicket.AssignedToUserId == null)
                return;

            if(oldTicket.Title != newTicket.Title)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    ReceipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Title has changed for Ticket Id {newTicket.Id} from {oldTicket.Title} to {newTicket.Title}."
                });
            }
            if (oldTicket.Description != newTicket.Description)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    ReceipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Description has changed for Ticket Id {newTicket.Id} from {oldTicket.Description} to {newTicket.Description}."
                });
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    ReceipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Priority has changed for Ticket Id {newTicket.Id} from {oldTicket.TicketPriority.Name} to {newTicket.TicketPriority.Name}."
                });
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    ReceipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Status has changed for Ticket Id {newTicket.Id} from {oldTicket.TicketStatus.Name} to {newTicket.TicketStatus.Name}."
                });
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    ReceipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Type has changed for Ticket Id {newTicket.Id} from {oldTicket.TicketType.Name} to {newTicket.TicketType.Name}."
                });
            }




        }

        public static void ManageAttachmentNotifications(TicketAttachment ticketattachment)
        {

        }


        private static void GenerateNotification(TicketNotification notification)
        {
            var db = new ApplicationDbContext();
            var newNotification = new TicketNotification
            {
                Created = DateTime.Now,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                ReceipientId = notification.ReceipientId,
                NotificationBody = notification.NotificationBody,
                HasBeenRead = false,
                TicketId = notification.TicketId,
            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();

        }
    }
}