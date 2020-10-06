using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData.ViewModel
{
    public class StudentViewModel
    {
        public Student Student { get; set; }

        public List<string> Courses { get; set; }

    }
}
