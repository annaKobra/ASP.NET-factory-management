using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Factory.Web.Models
{
    public class EmployeeShiftUtils
    {
        FactoryDBEntities db = new FactoryDBEntities();

        // can be one/many/none shifts
        public List<ShiftsForEmployeeDTO> GetShiftsForEmployee()
        {
            var query = from emp in db.employees
                        join empShifts in db.employeeShifts on emp.ID equals empShifts.EmployeeID
                        join s in db.shifts on empShifts.ShiftID equals s.ID
                        select new ShiftsForEmployeeDTO
                        {
                            EmpId = emp.ID,
                            EmpFullName = emp.FirstName + " " + emp.LastName,
                            ShiftId = s.ID,
                            DateAndTimeShift = s.ShiftDate + "  " + s.StartTime + ":00-" + s.EndTime + ":00"
                        };
            return query.ToList();
        }
        public void AddShift(int empId, int shiftId)
        {
            var newShift = new employeeShift() { 
                EmployeeID = empId,
                ShiftID = shiftId
            };
            db.employeeShifts.Add(newShift);
            db.SaveChanges();
        }

    }
}