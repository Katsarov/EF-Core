using EfCoreIntroductionDemo.Models;
using System;
using System.Linq;

namespace EfCoreIntroductionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();
            //Console.WriteLine(db.Employees.Count());
            //Console.WriteLine(db.Departments.Count());
            //Console.WriteLine($"Number of projects: {db.Projects.Count()}");

            var topProjects = db.Projects
                .OrderByDescending(p => p.EmployeesProjects.Count())
                .Select(x => new { x.Name, Count = x.EmployeesProjects.Count()})
                .Take(29).ToList();

            foreach (var project in topProjects)
            {
                Console.WriteLine(project.Name + " " + project.Count);
            }
        }
    }
}
