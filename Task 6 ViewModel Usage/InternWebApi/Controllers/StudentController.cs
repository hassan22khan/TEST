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

            var studentViewModels = service.GetStudentCourses(students);

            return Ok(studentViewModels);
        }
        
        [HttpPost]
        public IHttpActionResult PostStudent(StudentViewModel viewModel)
        {
            /*Recieved the viewModel from the user then saved student in Students table 
            and used the course ids to save data inside the studentCourses table.*/

            if (viewModel.Student == null) return BadRequest();
            
            service.Post(viewModel.Student);
            service.AddStudentCourses(viewModel);
            return Ok("posted student");
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            service.Delete(id);
            service.DeleteStudentCourses(id);
            return Ok("Deleted student");
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(StudentViewModel viewModel)
        {
            if (viewModel.Student == null) return BadRequest();
            service.Update(viewModel.Student);
            service.UpdatedStudentCourses(viewModel);
            return Ok("updated the student");
        }

    }
}
