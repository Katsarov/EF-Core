using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            this.HomeworkSubmissions = new HashSet<Homework>();
            this.Resources = new HashSet<Resource>();
            this.StudentsEnrolled = new HashSet<StudentCourse>();
        }

        public int CourseId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<Homework> HomeworkSubmissions { get; set; }

        public ICollection<Resource> Resources { get; set; }

        //[NotMapped]
        public ICollection<StudentCourse> StudentsEnrolled { get; set; }
    }
}


//o CourseId
//o	Name (up to 80 characters, unicode)
//o Description(unicode, not required)
//o StartDate
//o	EndDate
//o	Price

