using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Models;
using Repository;
using StudentData.ViewModel;

namespace Repository
{
    public class StudentRepository : BaseRepository<Student>
    {
        
        public StudentRepository(StudentContext context) : base(context)
        {
        }
        
        public IEnumerable<StudentViewModel> GetStudentCourses(IEnumerable<Student> students)
        {
            var studentViewModels = new List<StudentViewModel>();
            foreach (var student in students)
            {
                using (var context = new StudentContext())
                {
                    var coursesOfStudent = context.StudentCourses
                        .Where(sc => sc.StudentId == student.Id)
                        .Select(sc => sc.CourseId.ToString())
                        .ToList();

                    var viewModel = new StudentViewModel { Student = student, Courses = coursesOfStudent };

                    studentViewModels.Add(viewModel);
                }
            }
            return studentViewModels;
        }
        
        public void AddStudentCourses(StudentViewModel viewModel)
        {
            using (var context = new StudentContext())
            {
                foreach (var course in viewModel.Courses)
                {
                    context.StudentCourses
                        .Add(new StudentCourse { StudentId = viewModel.Student.Id, CourseId = int.Parse(course) });

                }
                context.SaveChanges();
            }

        }

        public void DeleteStudentCourses(int id)
        {
            using (var context = new StudentContext())
            {
                var studentCourses = context.StudentCourses.Where(sc => sc.StudentId == id).ToList();
                foreach (var studentCourse in studentCourses)
                {
                    context.StudentCourses.Remove(studentCourse);
                }
                context.SaveChanges();
            }
        }

        public void UpdatedStudentCourses(StudentViewModel viewModel)
        {
            using (var context = new StudentContext())
            {
                var studentCourses = context.StudentCourses.Where(sc => sc.StudentId == viewModel.Student.Id).ToList();
                foreach (var studentCourse in studentCourses)
                {
                    context.StudentCourses.Remove(studentCourse);
                }
                foreach (var course in viewModel.Courses)
                {
                    context.StudentCourses.Add(new StudentCourse { StudentId = viewModel.Student.Id, CourseId = int.Parse(course) });
                }
                context.SaveChanges();
            }
        }
    }
}
