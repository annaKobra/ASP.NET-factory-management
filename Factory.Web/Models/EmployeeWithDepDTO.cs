using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Factory.Web.Models
{
    public class EmployeeWithDepDTO
    {
        public int EmpId { get; set; }
        public int DepId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Year Started")]
        public int YearStarted { get; set; }
        public string DepName { get; set; }
    }
}