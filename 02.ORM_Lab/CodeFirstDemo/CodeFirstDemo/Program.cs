using CodeFirstDemo.Models;
using System;
using System.Collections.Generic;

namespace CodeFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();

            db.Categories.Add(new Category
            {
                Title = "Sport",
                News = new List<News>
                {
                    new News
                    {
                       Title = "Beroe bie Ludogorez",
                       Content = "Берое бие Лудогорец",
                       Comments = new List<Comment>
                       {
                           new Comment{ Autor = "Niki", Content = "da"},
                           new Comment{ Autor = "Kalin", Content = "da,da,da"}
                       }
                    }
                }
            });

            db.SaveChanges();
        }
    }
}
