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
            return Ok(students);
        }
        
        [HttpPost]
        public IHttpActionResult PostStudent(Student student)
        {
            if (student == null) return BadRequest();
            service.Post(student);
            return Ok("posted student");
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            service.Delete(id);
            return Ok("Deleted student");
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(Student student)
        {
            if (student == null) return BadRequest();
            service.Update(student);
            return Ok("updated the student");
        }

    }
}
