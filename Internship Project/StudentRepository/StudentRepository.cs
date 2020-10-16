using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Models;
using Repository;
using Data.ViewModel;
using StudentData.ViewModel;
using System.Data.Entity;

namespace Repository
{
    public class StudentRepository : BaseRepository<Student>
    {
        public IEnumerable<Student> GetStudentsPerPage(DataTablesParam param)
        {
            using (var context = new StudentContext())
            {
                return context.Students.OrderBy(s => s.Name).Skip(param.Start).Take(param.Length).ToList();
            }
        }

        public StudentViewModel GetOneStudentWithCourses(Student student) {
            using (var context = new StudentContext())
            {
                return new StudentViewModel
                {
                    Student = student,
                    Courses = context.StudentCourses
                    .Where(sc => sc.StudentId == student.Id)
                    .Select(sc => sc.CourseId.ToString())
                    .ToList()
                };
            }
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
        
        public IEnumerable<StudentListViewModel> GetStudentListWithCourses (IEnumerable<Student> students) 
        {
            var studentListViewModels = new List<StudentListViewModel>();
            foreach (var student in students)
            {
                using (var context = new StudentContext())
                {
                    var coursesOfStudent = context.StudentCourses
                        .Where(sc => sc.StudentId == student.Id)
                        .Include(sc => sc.Courses)
                        .Select(sc => sc.Courses.Name)
                        .ToList();

                    var viewModel = new StudentListViewModel { Student = student, Courses = coursesOfStudent };
                    studentListViewModels.Add(viewModel);
                }
            }
            return studentListViewModels;
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

        public decimal AddStudentByStoredProcedure(Student student)
        {
            using (var context = new StudentContext())
            {
                return context.InsertStudent(student);
            }
        }

        public void AddStudentCoursesByStoreProcedure(StudentViewModel studentViewModel,decimal id)
        {
            using (var context = new StudentContext())
            {
                context.InsertStudentCourses(studentViewModel,id);
            }
        }
    }
}
