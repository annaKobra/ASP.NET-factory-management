using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Factory.Web.Models
{
    public class DepartmentDTO
    {
        public int DepId { get; set; }
        [Display(Name = "Manger")]
        public string ManagerFullName { get; set; }
        [Display(Name = "Department")]
        [Required]
        public string DepartmentName { get; set; }
        public bool isExist { get; set; }
    }
}