using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanopy.Models
{
    public class ProjectsIndexViewModel
    {
        public Project Project { get; set; }
        public ICollection<Project> Projects { get; set; }
        public AssignProjectViewModel Assign { get; set; }
        public RemoveProjectViewModel Remove { get; set; }
    }
}