using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class StudentCourse
    {
        [Key]
        [Column(Order = 1)]
        public int StudentId { get; set; }

        public Student Student { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CourseId { get; set; }

        public Course Courses { get; set; }
    }
}
