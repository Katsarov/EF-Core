using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DemoTwo.Models
{
    [Keyless]
    public partial class NamesWithSalary
    {
        [Required]
        [Column("Full Name")]
        [StringLength(101)]
        public string FullName { get; set; }
        [Column(TypeName = "money")]
        public decimal Salary { get; set; }
    }
}
