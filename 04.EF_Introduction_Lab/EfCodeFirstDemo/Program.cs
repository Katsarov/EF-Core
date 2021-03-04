using EfCodeFirstDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EfCodeFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SliDoDbContext();

            db.Database.Migrate();
        }
    }
}
