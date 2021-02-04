using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Factory.Web.Models
{
    public class ShiftsForEmployeeDTO
    {
        public int EmpId { get; set; }
        public string EmpFullName { get; set; }
        public int ShiftId { get; set; }
        public string DateAndTimeShift { get; set; }

    }
}