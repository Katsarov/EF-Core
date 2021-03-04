using System;
using System.Text;
using SoftUni.Models;
using SoftUni.Data;
using System.Linq;
using System.Globalization;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new SoftUniContext();

            //var result = GetEmployeesFullInformation(db);
            //var result = GetEmployeesWithSalaryOver50000(db);
            //var result = AddNewAddressToEmployee(db);
            //var result = GetEmployeesFromResearchAndDevelopment(db);
            //var result = GetEmployeesInPeriod(db);
            //var result = GetAddressesByTown(db);
            //var result = GetEmployee147(db);
            //var result = GetDepartmentsWithMoreThan5Employees(db);
            //var result = GetLatestProjects(db);
            //var result = IncreaseSalaries(db);
            var result = RemoveTown(db);
            Console.WriteLine(result);
        }

        //---P15
        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns
                .First(t => t.Name == "Seattle");

            IQueryable<Address> addressesToDelete = context.Addresses
                .Where(a => a.TownId == townToDelete.TownId);

            int addressesCount = addressesToDelete.Count();

            IQueryable<Employee> emplOnDeletedAddresses = context.Employees
                .Where(e => addressesToDelete
                    .Any(a => a.AddressId == e.AddressId));

            foreach (Employee e in emplOnDeletedAddresses)
            {
                e.AddressId = null;
            }

            foreach (Address address in addressesToDelete)
            {
                context.Addresses.Remove(address);
            }

            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            return $"{addressesCount} addresses in {townToDelete.Name} were deleted";
        }

        //---P12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            IQueryable<Employee> empSalaryToIncrease = context.Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services");

            foreach (Employee employee in empSalaryToIncrease)
            {
                employee.Salary *= 1.12m;
            }

            context.SaveChanges();

            var employeesInfo = empSalaryToIncrease
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var e in employeesInfo)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P11
        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    Name = p.Name,
                    Desctiption = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .OrderBy(project => project.Name)
                .ToList();

            foreach (var project in projects)
            {
                //sb.AppendLine($"{project.Name}" +$"{Environment.NewLine}{project.Desctiption}" +$"{Environment.NewLine}{project.StartDate}");
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Desctiption}");
                sb.AppendLine($"{project.StartDate}");
            }

            var result = sb.ToString().Trim();
            return result;
        }

        //---P10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count() > 5)
                .OrderBy(d => d.Employees.Count())
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    DepartmentEmployees = d.Employees
                        .Select(e => new
                        {
                            EmployeeFirstName = e.FirstName,
                            EmployeeLastName = e.LastName,
                            e.JobTitle
                        })
                        .OrderBy(e => e.EmployeeFirstName)
                        .ThenBy(e => e.EmployeeLastName)
                        .ToList()
                })

                .ToList();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.ManagerFirstName}  {d.ManagerLastName}");

                foreach (var e in d.DepartmentEmployees)
                {
                    sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.JobTitle}");
                }
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    EmployeeID = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    ProjectsIn = e.EmployeesProjects
                        .Select(ep => ep.Project.Name)
                        .OrderBy(ep => ep)
                        .ToList()
                })
                .FirstOrDefault(e => e.EmployeeID == 147);

            sb.AppendLine($"{employees.FirstName} {employees.LastName} - {employees.JobTitle}");

            foreach (var p in employees.ProjectsIn)
            {
                sb.AppendLine($"{p}");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addressesEmployees = context.Addresses
                .OrderByDescending(e => e.Employees.Count())
                .ThenBy(t => t.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    AdresText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .ToList();

            foreach (var a in addressesEmployees)
            {
                sb
                    .AppendLine($"{a.AdresText}, {a.TownName} - {a.EmployeesCount} employees");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                    .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project
                                .StartDate
                                .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = ep.Project.EndDate.HasValue ?
                                ep.Project
                                .EndDate
                                .Value
                                .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                        })
                        .ToList()
                })
                .ToList();

            foreach (var e in employees)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                //--Half-Finger Gloves - 6/1/2002 12:00:00 AM - 6/1/2003 12:00:00 AM
                foreach (var p in e.Projects)
                {
                    sb
                        .AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            //Сега трябва да го закачим към Наков
            Employee nakov = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            nakov.Address = newAddress;

            context.SaveChanges();

            var addresses = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            foreach (var address in addresses)
            {
                sb
                    .AppendLine(address);
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(d => d.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var e in employees)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50_000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                sb
                    .AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //---P03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                })
                .OrderBy(e => e.EmployeeId)
                .ToList();

            foreach (var employee in employees)
            {
                sb
                    .AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            var result = sb.ToString().TrimEnd();

            return result;

        }
    }
}
