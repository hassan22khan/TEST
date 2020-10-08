using Data.Model;
using Repository;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.ViewModel;
using Models;

namespace StudentMVC.Controllers
{
    public class StudentController : Controller
    {
        StudentRepository service = new StudentRepository(new StudentContext());
        CoursesRepository courseService = new CoursesRepository(new StudentContext());
        // GET: Student
        
        public ActionResult StudentList()
        {
            var students = service.GetAll();
            var studentViewModels = service.GetStudentCourses(students).ToList();
            var courses = courseService.GetAll().ToList();

            var studentCourseViewModel = new StudentCourseViewModel 
                { StudentViewModels = studentViewModels , Courses = courses };
            
            return View(studentCourseViewModel);
        }

        public ActionResult StudentForm()
        {
            var courses = courseService.GetAll();
            var viewModel = new StudentFormViewModel { CoursesList = courses.ToList()};
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddStudent(StudentFormViewModel studentFormViewModel)
        {
            var studentViewModel = new StudentViewModel { Student = studentFormViewModel.Student, Courses = studentFormViewModel.Courses };
            if (studentFormViewModel.Student.Id == 0)
            {              
                service.Post(studentFormViewModel.Student);
                service.AddStudentCourses(studentViewModel);
            }
            else
            {
                service.Update(studentFormViewModel.Student);
                service.UpdatedStudentCourses(studentViewModel);
            }

            return RedirectToAction("StudentList");
        }

        public ActionResult EditStudent(int id)
        {
            var students = service.GetAll();
            var studentViewModels = service.GetStudentCourses(students).ToList();
            var viewModel = studentViewModels.SingleOrDefault(svm => svm.Student.Id == id);
            var studentFormViewModel = new StudentFormViewModel
            {
                Student = viewModel.Student,
                CoursesList = courseService.GetAll().ToList(),
                Courses = viewModel.Courses
            };
            return View("StudentForm", studentFormViewModel);
            
        }

        public ActionResult DeleteStudent(int id)
        {
            service.Delete(id);
            service.DeleteStudentCourses(id);
            return RedirectToAction("StudentList");
        }
    }
}