using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new List<Student>()
            {
                new Student{Name = "Niki", Marks = new List<int>{2,3,2,3,2}},
                new Student{Name = "Stoyan", Marks = new List<int>{ 6,5,6,5,6}},
                new Student{Name = "Nivo", Marks = new List<int>{ 6,6,6}}
            };

            var groups = collection
                //.Where(x => x.Marks.Average() >= 5)
                .Select(x => new StudentProjection
                {
                    Name = x.Name,
                    NameInitial = x.Name.Substring(0, 1),
                    AverageMarks = x.Marks.Average(),
                })
                .OrderByDescending(x => x.AverageMarks)
                .GroupBy(x => x.NameInitial);

            //var collection2 = collection.Where(x => x.Name.StartsWith("N"));

            //var collection2 = collection.Where(Predicate);
            foreach (var group in groups)
            {
                //Console.WriteLine($"{student.Name} > {student.NameInitial} = {student.AverageMarks}");
                Console.WriteLine($"{group.Key} {group.Count()}");
            }
        }

        //  ------------------- Стандартен метод -------------------
        //static bool Predicate(Student student)
        //{
        //    if (student.Marks.Average() >= 5)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //  ------------------- Същата функция с предикат -------------------
        //static bool Predicate(Student student)
        //{
        //    return student.Marks.Average() >= 5;
        //}

        // -----------------------------------------------------------------
    }

    class StudentProjection
    {
        public string Name { get; set; }

        public string NameInitial { get; set; }

        public double AverageMarks { get; set; }
    }

    class Student
    {
        public string Name { get; set; }

        public List<int> Marks { get; set; }
    }
}
