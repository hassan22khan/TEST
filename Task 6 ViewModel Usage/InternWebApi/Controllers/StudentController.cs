using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Repository;
using Models;
using Newtonsoft.Json;
using StudentData.ViewModel;

namespace InternWebApi.Controllers
{
    public class StudentController : ApiController
    {
       
        StudentRepository service = new StudentRepository(new StudentContext());
       
        [HttpGet]
        public IHttpActionResult GetStudents()
        {
            var students = service.GetAll();
            if (students == null) return NotFound();
            var StudentViewModels = new List<StudentViewModel>();
            foreach(var student in students)
            {
                using (var context = new StudentContext())
                {
                    var coursesOfStudent = context.StudentCourses
                        .Where(sc => sc.StudentId == student.Id)
                        .Select(sc => sc.CourseId.ToString())
                        .ToList();

                    var viewModel = new StudentViewModel { Student = student, Courses = coursesOfStudent};

                    StudentViewModels.Add(viewModel);
                }
            }
            return Ok(StudentViewModels);
        }
        
        [HttpPost]
        public IHttpActionResult PostStudent(StudentViewModel viewModel)
        {
            /*Recieved the viewModel from the user then saved student in Students table 
            and used the course ids to save data inside the studentCourses table.*/

            if (viewModel.Student == null) return BadRequest();
            
            service.Post(viewModel.Student);
            
            using (var context = new StudentContext())
            {
                foreach (var course in viewModel.Courses)
                {
                    context.StudentCourses.Add(new StudentCourse { StudentId = viewModel.Student.Id, CourseId = int.Parse(course) });
                    
                }
                context.SaveChanges();
            }
            
            return Ok("posted student");
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            service.Delete(id);
            using (var context = new StudentContext())
            {
               var studentCourses = context.StudentCourses.Where(sc => sc.StudentId == id).ToList();
                foreach(var studentCourse in studentCourses)
                {
                    context.StudentCourses.Remove(studentCourse);
                }
                context.SaveChanges();
            }
                return Ok("Deleted student");
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(StudentViewModel viewModel)
        {
            if (viewModel.Student == null) return BadRequest();
            service.Update(viewModel.Student);
            using (var context = new StudentContext())
            {
                var studentCourses = context.StudentCourses.Where(sc => sc.StudentId == viewModel.Student.Id).ToList();
                foreach(var studentCourse in studentCourses)
                {
                    context.StudentCourses.Remove(studentCourse);
                }
                foreach(var course in viewModel.Courses)
                {
                    context.StudentCourses.Add(new StudentCourse { StudentId = viewModel.Student.Id, CourseId = int.Parse(course) });
                }
                context.SaveChanges();
            }
                return Ok("updated the student");
        }

    }
}
