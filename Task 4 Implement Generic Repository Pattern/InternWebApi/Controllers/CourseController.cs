using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InternWebApi.Controllers
{
    public class CourseController : ApiController
    {
        CoursesRepository service = new CoursesRepository();

        [HttpGet]
        public IHttpActionResult GetAllCourses()
        {
            var courses = service.GetAllCourses();
            if (courses == null) return NotFound();
            return Ok(courses);
        }

        [HttpGet]
        public IHttpActionResult GetCourse(int id) {
            var course = service.GetCourse(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost] 
        public IHttpActionResult AddCourse(Course course)
        {
            if (course == null) return BadRequest();
            service.AddCourse(course);
            return Ok("Posted the student");
        }

        [HttpPut]
        public IHttpActionResult UpdateCourse(int id,Course course)
        {
            service.UpdateCourse(id,course);
            return Ok("Updated the course");
        }

        [HttpDelete]
        public IHttpActionResult DeleteCourse(int id)
        {
            service.DeleteCourse(id);
            return Ok("deleted the student");
        }
    }
}
