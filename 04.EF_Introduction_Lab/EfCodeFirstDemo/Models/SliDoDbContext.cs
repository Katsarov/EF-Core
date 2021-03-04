using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfCodeFirstDemo.Models
{
    public class SliDoDbContext : DbContext
    {
        public SliDoDbContext()
        {

        }

        public SliDoDbContext(DbContextOptions dbContextOptions)
            :base(dbContextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SliDo;Integrated Security=true");
            }
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Questions> Questions { get; set; }
    }
}
