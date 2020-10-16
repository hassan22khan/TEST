using Microsoft.AspNet.Identity;
using Repository;
using StudentRepository.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace StudentMVC.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        StudentsRepository studentService = new StudentsRepository();
        CoursesRepository courseService = new CoursesRepository();
        
        public ActionResult StudentList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetStudentsInJson(DataTablesParam param)
        {
            var studentListViewModels = studentService.GetStudentListWithCourses(studentService.GetStudentsPerPage(param,User.Identity.GetUserId()));
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
            studentFormViewModel.Student.UserId = User.Identity.GetUserId();
            /* if (!ModelState.IsValid || studentFormViewModel.Student.Password != studentFormViewModel.Student.ConfirmPassword)
             {
                 studentFormViewModel.CoursesList = courseService.GetAll().ToList();
                 return View("StudentForm", studentFormViewModel);
             }*/
            var studentViewModel = new StudentViewModel { Student = studentFormViewModel.Student, Courses = studentFormViewModel.Courses };
            if (studentFormViewModel.Student.Id == 0)
            {
                // Managed by Entity Framework          
                studentService.Post(studentFormViewModel.Student);
                studentService.AddStudentCourses(studentViewModel);
                //Managed By Stored Procedures
                //decimal id = studentService.AddStudentByStoredProcedure(studentFormViewModel.Student);
                //if(id != 0) studentService.AddStudentCoursesByStoreProcedure(studentViewModel,id);
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