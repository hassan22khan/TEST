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
        StudentRepository studentService = new StudentRepository(new StudentContext());
        CoursesRepository courseService = new CoursesRepository(new StudentContext());
        
        public ActionResult StudentList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetStudentsInJson()
        {
            
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            var studentListViewModels = studentService.GetStudentListWithCourses(studentService.GetAll());
            
            return Json(
                new {
                    draw = Request.Form.GetValues("draw").FirstOrDefault(),
                    recordsFiltered = studentListViewModels.Count(),
                    recordsTotal = studentListViewModels.Count(),
                    data = studentListViewModels.Skip(skip).Take(pageSize).ToList()
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
            var studentViewModel = new StudentViewModel { Student = studentFormViewModel.Student, Courses = studentFormViewModel.Courses };
            if (studentFormViewModel.Student.Id == 0)
            {              
                studentService.Post(studentFormViewModel.Student);
                studentService.AddStudentCourses(studentViewModel);
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