using Demo.Models;
using System;
using System.Linq;

namespace Demo
{
    public class Program
    {
        //Microsoft.EntityFrameworkCore.SqlServer

        static void Main(string[] args)
        {
            //var db = new SoftUniContext();

            //Да вземем всички слувители, на които имената започват с "N", да ги сортираме по заплата (DESC), да ги селектираме по име, фамилия и заплата и да ги направим на списък
            //-------------------------
            //var employees = db.Employees
            //    .Where(x => x.FirstName
            //    .StartsWith("N"))
            //    .OrderByDescending(x => x.Salary)
            //    .Select(x => new { x.FirstName, x.LastName, x.Salary })
            //    .ToList();
            //int num = 1;
            //foreach (var employee in employees)
            //{

            //    Console.WriteLine($"{num}. {employee.FirstName} {employee.LastName} => {employee.Salary}");
            //    num++;
            //}


            //Да получим за всеки един отдел бройката на служителите в него
            //--------------------------
            var db = new SoftUniContext();
            var departments = db.Employees
                .GroupBy(x => x.Department.Name)
                .Select(x => new { Name = x.Key, Count = x.Count() })
                .ToList();

            foreach (var department in departments)
            {
                Console.WriteLine($"{department.Name} => {department.Count}");
            }

        }
    }
}
