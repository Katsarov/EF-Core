using System;
using DemoInsertTown.Models;


namespace DemoInsertTown
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            //Добавяне на нов град
            //---------------------------
            db.Towns.Add(new Town { Name = "Razgrad" });
            db.SaveChanges();
        }
    }
}
