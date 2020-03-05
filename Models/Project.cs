using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kanopy.Models
{
    public class Project
    {
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string ProjectManagerId { get; set; }
        public string ProjectManager { get; set; }
        public string FirstName { get; set; }



        //Ticket Status ints
        public int New { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int OnHold { get; set; }


        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
    public class AssignProjectViewModel
    {
        [Display(Name = "User")]
        [Required(ErrorMessage = "User is Required.")]
        public SelectList UserId { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project is Required.")]
        public SelectList ProjectId { get; set; }
        public virtual ICollection<ApplicationUser> UserRoles { get; set; }

    }

    public class RemoveProjectViewModel
    {
        public SelectList UserId { get; set; }
        public SelectList ProjectId { get; set; }
        public virtual ICollection<ApplicationUser> UserRoles { get; set; }

    }
}