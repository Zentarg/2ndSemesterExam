using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class SalaryHandler
    {
        public static Dictionary<int, Salary> GetAllSalaries(ParknGardenData db)
        {
            Dictionary<int, Salary> salaries = new Dictionary<int, Salary>();

            List<Salary> userDB = db.Salaries.ToList();

            foreach (Salary salary in userDB)
            {
                salaries.Add(salary.UserID, salary);
            }

            return salaries;

        }

        public static Salary GetOneSalary(int userID, ParknGardenData db)
        {
            return db.Salaries.FirstOrDefault(s => s.UserID == userID);
        }
    }
}