using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Factory.Web.Models
{
    public class ShiftUtils
    {
        FactoryDBEntities db = new FactoryDBEntities();
        public List<shift> GetAll()
        {
            return db.shifts.ToList();
        }
        public void Add(shift newShift)
        {
            db.shifts.Add(newShift);
            db.SaveChanges();
        }
    }
}