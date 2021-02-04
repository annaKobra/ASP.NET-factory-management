using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Factory.Web.Models
{
    public class EmployeeUtils
    {
        FactoryDBEntities db = new FactoryDBEntities();
        public List<EmployeeWithDepDTO> GetEmployeesWithDepartment()
        {
            var query = from emp in db.employees
                        join dep in db.departments
                        on emp.DepID equals dep.ID
                        select new EmployeeWithDepDTO { 
                            DepId = dep.ID,
                            DepName = dep.DepName,
                            EmpId = emp.ID,
                            FirstName = emp.FirstName,
                            LastName = emp.LastName,
                            YearStarted = emp.YearStarted
                        };
            return query.ToList();
        }
        public List<employee> GetEmployees()
        {
            return db.employees.ToList();
        }
        public employee GetById(int empId)
        {
            return db.employees.FirstOrDefault(emp => emp.ID == empId);
        }
        public void Update(employee employee)
        {
            var existing = db.employees.FirstOrDefault(e => e.ID == employee.ID);
            if (existing != null)
            {
                existing.FirstName = employee.FirstName;
                existing.LastName = employee.LastName;
                existing.YearStarted = employee.YearStarted;
                existing.DepID = employee.DepID;

                db.SaveChanges();
            }
        }
        public void Delete(int empId)
        {
            // Remove emp shifts without using foreach loop
            //var employees = db.employeeShifts.RemoveRange(db.employeeShifts.Where(emp => emp.EmployeeID == empId).ToList());
            
            // find related shifts
            var employeeShifts = db.employeeShifts.Where(emp => emp.EmployeeID == empId).ToList();
            foreach(var shift in employeeShifts)
            {
                db.employeeShifts.Remove(shift);
            }
            var existing = db.employees.Find(empId);
            db.employees.Remove(existing);
            db.SaveChanges();
        }
        // use in department n delete
        /*        public string GetEmployeeName(int managerId)
                {
                    // Find uniq record
                    var employee = db.employees.SingleOrDefault(emp => emp.ID == managerId);
                    var name = employee != null ? employee.FirstName + employee.LastName : String.Empty;
                    return name;
                }*/
        public string GetEmployeeName(int empId)
        {
            var employee = GetById(empId);
            var name = employee != null ? employee.FirstName + " " + employee.LastName : String.Empty;
            return name;
        }
      /*  public List<employee> GetSearchByEmpName(string searchName)
        {
            searchName = searchName.ToLower();
            var empPhaseName = GetEmployees().Where(e => e.FirstName.ToLower().Contains(searchName) || e.LastName.ToLower().Contains(searchName));

            return empPhaseName.ToList();
        }*/
/*        public List<employee> GetSearchByDepName(string searchName)
        {
            searchName = searchName.ToLower();
            var searchedListByDep = GetAll().Where(e => e.DepartmentName.ToLower().Contains(searchName)).ToList();

            List<employee> empDepPhaseName = new List<employee>();

            foreach (var dep in searchedListByDep)
            {
                foreach (var emp in db.employees.ToList())
                {
                    if (dep.DepId == emp.DepID)
                    {
                        empDepPhaseName.Add(emp);
                    }
                }
            }
            return empDepPhaseName.ToList();
        }*/
        public List<EmployeeWithDepDTO> Search(string searchName)
        {
            searchName = searchName.ToLower();
            var employees = GetEmployeesWithDepartment();

            var searchedList = employees.Where(emp => emp.FirstName.ToLower().Contains(searchName) ||
                                                      emp.LastName.ToLower().Contains(searchName) ||
                                                      emp.DepName.ToLower().Contains(searchName));

            return searchedList.ToList();
        }
    }
}