using Repository;
using StudentRepository.ViewModels;
using System;
using System.Security.Claims;
using System.Web.Http;

namespace InternWebApi.Controllers
{
    [Authorize]
    public class StudentController : ApiController
    {
       
        StudentsRepository service = new StudentsRepository();
       
        [HttpGet]
        public IHttpActionResult GetStudents(string id)
        {
            var students = service.GetStudentsForEachUser(id);
            if (students == null) return NotFound();

            var studentViewModels = service.GetStudentCourses(students);

            return Ok(studentViewModels);
        }
        
        [HttpPost]
        public IHttpActionResult PostStudent(StudentViewModel viewModel)
        {
            if (viewModel.Student == null) return BadRequest();
            //Adding UserId to Student Table
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
