using Data.Model;
using Repository;
using Data.ViewModel;
using System.Linq;
using System.Web.Mvc;
using StudentData.ViewModel;
using System;

namespace StudentMVC.Controllers
{
    public class StudentController : Controller
    {
        StudentRepository studentService = new StudentRepository();
        CoursesRepository courseService = new CoursesRepository();
        
        public ActionResult StudentList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetStudentsInJson(DataTablesParam param)
        {
            var studentListViewModels = studentService.GetStudentListWithCourses(studentService.GetStudentsPerPage(param));
            return Json(
                new {
                    draw = param.Draw,
                    recordsFiltered = studentListViewModels.Count(),
                    recordsTotal = studentListViewModels.Count(),
                    data = studentListViewModels
                }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentForm()
        {
            return View(new StudentFormViewModel { CoursesList = courseService.GetAll().ToList() });
        }

        [HttpPost]
        public ActionResult AddStudent(StudentFormViewModel studentFormViewModel)
        {
           /* if (!ModelState.IsValid || studentFormViewModel.Student.Password != studentFormViewModel.Student.ConfirmPassword)
            {
                studentFormViewModel.CoursesList = courseService.GetAll().ToList();
                return View("StudentForm", studentFormViewModel);
            }*/

            var studentViewModel = new StudentViewModel { Student = studentFormViewModel.Student, Courses = studentFormViewModel.Courses };
            if (studentFormViewModel.Student.Id == 0)
            {
                // Managed by Stored Procedure          
                //studentService.Post(studentFormViewModel.Student);
                //studentService.AddStudentCourses(studentViewModel);
               
                //Managed By Stored Procedures
                decimal id = studentService.AddStudentByStoredProcedure(studentFormViewModel.Student);
                if(id != 0) studentService.AddStudentCoursesByStoreProcedure(studentViewModel,id);
            }
            else
            {
                studentService.Update(studentFormViewModel.Student);
                studentService.UpdatedStudentCourses(studentViewModel);
            }
            return RedirectToAction("StudentList");
        }

        public ActionResult EditStudent(int id)
        {
            var studentViewModel = studentService.GetOneStudentWithCourses(studentService.GetOne(id));
            
            return View("StudentForm",
                new StudentFormViewModel
            {
                Student = studentViewModel.Student,
                CoursesList = courseService.GetAll().ToList(),
                Courses = studentViewModel.Courses
            });
        }

        public ActionResult DeleteStudent(int id)
        {
            studentService.DeleteStudentCourses(id);
            studentService.Delete(id);
            return RedirectToAction("StudentList");
        }
    }
}