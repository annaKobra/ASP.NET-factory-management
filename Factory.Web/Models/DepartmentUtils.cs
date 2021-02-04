using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Factory.Web.Models
{
    public class DepartmentUtils
    {
        FactoryDBEntities db = new FactoryDBEntities();
        public List<DepartmentDTO> GetAll()
        {
            var query = from dep in db.departments
                       join emp in db.employees on dep.Manager equals emp.ID
                       select new DepartmentDTO
                       {
                           DepId = dep.ID,
                           DepartmentName = dep.DepName,
                           ManagerFullName = emp.FirstName + " " + emp.LastName
                       };
            return query.ToList();
        }
        /*        public DepartmentDTO GetById(int depId)
                {
                    var allDepartments = GetAll();
                    return allDepartments.FirstOrDefault(department => department.DepId == depId);
                }*/
      /*  public void Update(DepartmentDTO department)
        {
            var existing = db.departments.FirstOrDefault(dep => dep.ID == department.DepId);
            if (existing != null)
            {
                existing.DepName = department.DepartmentName;
                // Find Employee because he is manger
                var employee = db.employees.FirstOrDefault(e => e.ID == existing.Manager);
                if (employee != null)
                {
                    employee.FirstName = department.ManagerFirstName;
                    employee.LastName = department.ManagerLastName;
                }
                db.SaveChanges();
            }
        }*/

        public department GetById(int depId)
        {
            return db.departments.FirstOrDefault(department => department.ID == depId);
        }
        //oPTION2 - with department model
        public void Update(department department)
        {
            var existing = db.departments.FirstOrDefault(dep => dep.ID == department.ID);
            if (existing != null)
            {
                existing.DepName = department.DepName;
                existing.Manager = department.Manager;
                db.SaveChanges();
            }
        }
        /*        public void Add(string newDepName, int id)
                {
                    var newDepartment = new department();

                    newDepartment.DepName = newDepName;
                    newDepartment.Manager = id;

                    db.departments.Add(newDepartment);
                    db.SaveChanges();
                }*/
        public void Add(department newDep)
        {
            var newDepartment = new department();

            newDepartment.DepName = newDep.DepName;
            newDepartment.Manager = newDep.Manager;

            db.departments.Add(newDepartment);
            db.SaveChanges();
        }
        public void Delete(int depId)
        {
            var department = db.departments.Find(depId);
           /// var department = db.departments.Where(d => d.ID == depId).First();
            db.departments.Remove(department);
            db.SaveChanges();
        }
        public bool IsEmployeeInDepartment(int depId)
        {
            var employee = db.employees.FirstOrDefault(e => e.DepID == depId);
            if (employee != null)
                return true;
            return false;
        }
        public string GetDepNameForEmp(int depId)
        {
            var linq = from dep in db.departments
                       where dep.ID == depId
                       select dep.DepName;

            return linq.Single();
        }
    }
}