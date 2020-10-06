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
        //key for this table will be a composite key including both foreign keys

        [Key]
        [Column(Order = 1)]                 //order 1 specifies here that StudentId will be placed first in the composite key
        public int StudentId { get; set; }

        public Student Student { get; set; }

        [Key]
        [Column(Order = 2)]                 //order 2 specifies here that StudentId will be placed first in the composite key
        public int CourseId { get; set; }

        public Course Courses { get; set; }
    }
}
