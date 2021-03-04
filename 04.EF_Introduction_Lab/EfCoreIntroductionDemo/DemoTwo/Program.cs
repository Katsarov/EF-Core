using DemoTwo.Models;
using System;
using System.Linq;

namespace DemoTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            var employees = db.Employees
                .Where(x => x.JobTitle == "Design Engineer")
                //.Select(x => x.FirstName)
                .OrderBy(x => x.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName}");
            }
        }
    }
}
