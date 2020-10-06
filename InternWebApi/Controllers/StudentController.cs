using StudentData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentRepository;
using StudentModels;
using Newtonsoft.Json;

namespace InternWebApi.Controllers
{
    public class StudentController : ApiController
    {
        
        StudentService service = new StudentService();
       
        [HttpGet]
        public IHttpActionResult GetStudents()
        {
            var students = service.GetStudents();
            if (students == null) return NotFound();
            return Ok();
        }
        
        [HttpPost]
        public IHttpActionResult PostStudent(Student student)
        {
            if (student == null) return BadRequest();
            service.AddStudent(student);
            return Ok("posted student");
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            service.DeleteStudent(id);
            return Ok("Deleted student");
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(Student student)
        {
            if (student == null) return BadRequest();
            service.UpdateStudent(student);
            return Ok("updated the student");
        }

    }
}
